using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace TareaGrafica
{
    internal class Game: GameWindow
    {
        private Escenario escenario;
        private float angle = 0.0f;
        private bool isMouseDown = false; // Para rastrear si el botón del mouse está presionado
        private Vector2 lastMousePos; // Última posición del mouse
        private float pitch = 0.0f; // Rotación alrededor del eje X
        private float yaw = 0.0f;   // Rotación alrededor del eje Y
        private float zoom = 2.0f;  // Distancia de la cámara al objeto


        public Game(int width, int height)
           : base(width, height, GraphicsMode.Default, "OpenTK Window")
        {
            VSync = VSyncMode.On; // Habilitar VSync para evitar el tearing
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            // Configuración de la proyección en perspectiva
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                Width / (float)Height,
                0.1f,  // near plane
                100.0f // far plane
            );
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            //List<Punto> puntosT = new List<Punto>
            //{
            //    // Parte superior de la T (barra horizontal - Frente)
            //    new Punto(-2.0f,  1.0f,  1.0f),  // Frente - Superior Izquierda // 0
            //    new Punto( 2.0f,  1.0f,  1.0f),  // Frente - Superior Derecha   // 1
            //    new Punto( 2.0f,  1.2f,  1.0f),  // Frente - Inferior Derecha   // 2
            //    new Punto(-2.0f,  1.2f,  1.0f),  // Frente - Inferior Izquierda // 3

            //    // Tronco de la T (barra vertical - Frente)
            //    new Punto(-0.5f, -1.0f,  1.0f),  // Frente - Inferior Izquierda // 4
            //    new Punto( 0.5f, -1.0f,  1.0f),  // Frente - Inferior Derecha   // 5
            //    new Punto( 0.5f,  1.0f,  1.0f),  // Frente - Superior Derecha   // 6
            //    new Punto(-0.5f,  1.0f,  1.0f),  // Frente - Superior Izquierda // 7

            //    // Parte superior de la T (barra horizontal - Atrás)  
            //    new Punto(-2.0f,  1.0f, -1.0f),  // Atrás - Superior Izquierda  // 8
            //    new Punto( 2.0f,  1.0f, -1.0f),  // Atrás - Superior Derecha    // 9
            //    new Punto( 2.0f,  1.2f, -1.0f),  // Atrás - Inferior Derecha    // 10
            //    new Punto(-2.0f,  1.2f, -1.0f),  // Atrás - Inferior Izquierda  // 11

            //    // Tronco de la T (barra vertical - Atrás)
            //    new Punto(-0.5f, -1.0f, -1.0f),  // Atrás - Inferior Izquierda  // 12
            //    new Punto( 0.5f, -1.0f, -1.0f),  // Atrás - Inferior Derecha    // 13
            //    new Punto( 0.5f,  1.0f, -1.0f),  // Atrás - Superior Derecha    // 14
            //    new Punto(-0.5f,  1.0f, -1.0f),  // Atrás - Superior Izquierda  // 15
            //};

            List<Punto> puntosT = LeerObj();


            List<Poligono> poligonosT = new List<Poligono>
            {

                // Parte superior de la T (Rectángulo Horizontal - Frente)
                new Poligono(new List<Punto> { puntosT[0], puntosT[1], puntosT[2], puntosT[3] }, 1.0f, 0.0f, 0.0f), // Rojo

                // Tronco de la T (Rectángulo Vertical - Frente)
                new Poligono(new List<Punto> { puntosT[4], puntosT[5], puntosT[6], puntosT[7] }, 0.0f, 1.0f, 0.0f), // Verde

                // Parte superior de la T (Rectángulo Horizontal - Atrás)
                new Poligono(new List<Punto> { puntosT[8], puntosT[9], puntosT[10], puntosT[11] }, 1.0f, 0.0f, 0.0f), // Rojo

                // Tronco de la T (Rectángulo Vertical - Atrás)
                new Poligono(new List<Punto> { puntosT[12], puntosT[13], puntosT[14], puntosT[15] }, 0.0f, 1.0f, 0.0f), // Verde


                // Conexiones laterales
                // Lado derecho superior de la T
                new Poligono(new List<Punto> { puntosT[1], puntosT[9], puntosT[10], puntosT[2] }, 0.0f, 0.0f, 1.0f), // Azul
                // Lado izquierdo superior de la T
                new Poligono(new List<Punto> { puntosT[0], puntosT[8], puntosT[11], puntosT[3] }, 1.0f, 1.0f, 0.0f), // Amarillo
                // Lado derecho del tronco de la T
                new Poligono(new List<Punto> { puntosT[5], puntosT[13], puntosT[14], puntosT[6] }, 0.0f, 1.0f, 1.0f), // Cyan
                // Lado izquierdo del tronco de la T
                new Poligono(new List<Punto> { puntosT[4], puntosT[12], puntosT[15], puntosT[7] }, 1.0f, 0.0f, 1.0f), // Magenta
                // Conexión entre la parte superior y el tronco (cara inferior)
                 new Poligono(new List<Punto> { puntosT[8], puntosT[9], puntosT[1], puntosT[0] }, 1.0f, 0.5f, 0.5f), // Naranja
                // Conexión entre la parte superior y el tronco (cara superior)
                new Poligono(new List<Punto> { puntosT[10], puntosT[11], puntosT[3], puntosT[2] }, 0.5f, 1.0f, 0.5f), // Verde claro
                // Piso de la T
                new Poligono(new List<Punto> {puntosT[12], puntosT[13], puntosT[5], puntosT[4] }, 0.5f, 1.0f, 0.5f)
            };


            Parte partesT = new Parte();
            foreach (var poligono in poligonosT)
            {
                partesT.AddPoligono(poligono);
            }

            Objeto objetoT = new Objeto();
            objetoT.Addparte(partesT);

            // Crear el escenario y agregar objetos
            escenario = new Escenario();
            escenario.AddObjeto("T",objetoT);
            // Guardar el objeto en un archivo .obj
            GuardarObj(puntosT);

        }

        protected void GuardarObj(List<Punto> puntos)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modelo.obj");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var punto in puntos)
                {
                    writer.WriteLine(punto.ToString());
                }

                // Puedes añadir información adicional aquí como normales y caras si es necesario
            }

            Console.WriteLine("Archivo guardado en: " + filePath);
        }

        protected List<Punto> LeerObj()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modelo.obj");

            List<Punto> puntos = new List<Punto>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    {
                        string[] parts = line.Split(' ');
                        float x = float.Parse(parts[0]);
                        float y = float.Parse(parts[1]);
                        float z = float.Parse(parts[2]);
                        puntos.Add(new Punto(x, y, z));
                    }
                }
            }

            foreach (var punto in puntos)
            {
                Console.WriteLine(punto.ToString());
            }

            return puntos;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Establecer la matriz de modelo/vista
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Aplicar la traslación para el zoom
            GL.Translate(0.0f, 0.0f, -zoom);

            // Aplicar la rotación de la vista basada en el mouse
            GL.Rotate(pitch, 1.0f, 0.0f, 0.0f); // Rotar alrededor del eje X
            GL.Rotate(yaw, 0.0f, 1.0f, 0.0f);   // Rotar alrededor del eje Y

            // Dibujar los ejes cartesianos
            GL.Begin(PrimitiveType.Lines);

            // Eje X en rojo
            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, 0.0f);

            // Eje Y en verde
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 1.0f, 0.0f);

            // Eje Z en azul
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 1.0f);

            GL.End();

            // Dibujar el escenario (y por ende el cubo)
            escenario.DibujarEscenario();
            SwapBuffers();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButton.Left)
            {
                isMouseDown = true;
                lastMousePos = new Vector2(e.X, e.Y);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButton.Left)
            {
                isMouseDown = false;
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            // Ajustar la distancia de la cámara (zoom) en función del scroll del mouse
            zoom -= e.DeltaPrecise * 0.5f;

            // Limitar el zoom para evitar que pase a través del objeto o se aleje demasiado
            if (zoom < 1.0f)
                zoom = 1.0f;
            if (zoom > 20.0f)
                zoom = 20.0f;
        }


        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            if (isMouseDown)
            {
                Vector2 delta = new Vector2(e.X, e.Y) - lastMousePos;
                lastMousePos = new Vector2(e.X, e.Y);

                yaw += delta.X * 0.5f;  // Ajustar sensibilidad
                pitch += delta.Y * 0.5f; // Invertir si quieres mover el mouse hacia abajo para mover la cámara hacia arriba
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height); // Configurar el viewport al tamaño de la ventana
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            // Lógica de actualización, como manejar la entrada del usuario, se coloca aquí
        }

    }
}
