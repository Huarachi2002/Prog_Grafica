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

            //List<Punto> puntosT = LeerObj();
            //List<Punto> puntosU = new List<Punto>
            //    {
            //        new Punto(-1, 1,  0.5f), // 0
            //        new Punto( 1, 1,  0.5f), // 1
            //        new Punto( 1, -1, 0.5f), // 2
            //        new Punto(-1, -1, 0.5f), // 3
            //        new Punto(-1, 1, -0.5f), // 4
            //        new Punto( 1, 1, -0.5f), // 5
            //        new Punto( 1, -1, -0.5f), // 6
            //        new Punto(-1, -1, -0.5f), // 7
            //        new Punto(-0.7f, 1,  0.5f), // 8
            //        new Punto( 0.7f, 1,  0.5f), // 9
            //        new Punto( 0.7f, -0.7f, 0.5f), // 10
            //        new Punto(-0.7f, -0.7f, 0.5f), // 11
            //        new Punto(-0.7f, 1, -0.5f), // 12
            //        new Punto( 0.7f, 1, -0.5f), // 13
            //        new Punto( 0.7f, -0.7f, -0.5f), // 14
            //        new Punto(-0.7f, -0.7f, -0.5f)  // 15
            //    };


            //List<Poligono> poligonosU = new List<Poligono>
            //    {
            //        // Parte trasera del lado izq
            //        new Poligono(new List<Punto> { puntosU[4], puntosU[12], puntosU[15], puntosU[7] }, 1.0f, 0.0f, 0.0f),
            //        // Parte trasera inferior
            //        new Poligono(new List<Punto> { puntosU[7], puntosU[15], puntosU[14], puntosU[6] }, 1.0f, 0.0f, 0.0f),
            //        // Parte trasera del lado der
            //        new Poligono(new List<Punto> { puntosU[13], puntosU[5], puntosU[6], puntosU[14] }, 1.0f, 0.0f, 0.0f),
            //        // Parte delantera del lado izq
            //        new Poligono(new List<Punto> { puntosU[0], puntosU[8], puntosU[11], puntosU[3] }, 0.0f, 1.0f, 0.0f),
            //        // Parte delantera inferior
            //        new Poligono(new List<Punto> { puntosU[3], puntosU[11], puntosU[10], puntosU[2] }, 0.0f, 1.0f, 0.0f),
            //        // Parte delantera del lado der
            //        new Poligono(new List<Punto> { puntosU[9], puntosU[1], puntosU[2], puntosU[10] }, 0.0f, 1.0f, 0.0f),
            //        // Lado izquierdo
            //        new Poligono(new List<Punto> { puntosU[0], puntosU[4], puntosU[7], puntosU[3] }, 0.0f, 0.0f, 1.0f),
            //        // Lado izquierdo der
            //        new Poligono(new List<Punto> { puntosU[8], puntosU[12], puntosU[15], puntosU[11] }, 0.0f, 0.0f, 1.0f),
            //        // Lado derecho
            //        new Poligono(new List<Punto> { puntosU[1], puntosU[5], puntosU[6], puntosU[2] }, 1.0f, 1.0f, 0.0f),
            //        // Lado derecho izq
            //        new Poligono(new List<Punto> { puntosU[9], puntosU[13], puntosU[14], puntosU[10] }, 1.0f, 1.0f, 0.0f),
            //        // Parte inferior abajo
            //        new Poligono(new List<Punto> { puntosU[3], puntosU[7], puntosU[6], puntosU[2] }, 1.0f, 0.0f, 1.0f),
            //        // Parte inferior arriba
            //        new Poligono(new List<Punto> { puntosU[11], puntosU[15], puntosU[14], puntosU[10] }, 1.0f, 0.0f, 1.0f),
            //        // Parte superior izq
            //        new Poligono(new List<Punto> { puntosU[0], puntosU[8], puntosU[12], puntosU[4] }, 0.0f, 1.0f, 1.0f),
            //        // Parte superior der
            //        new Poligono(new List<Punto> { puntosU[9], puntosU[1], puntosU[5], puntosU[13] }, 0.0f, 1.0f, 1.0f)
            //    };


                //Parte partesU = new Parte();
                //foreach (var poligono in poligonosU)
                //{
                //    partesU.AddPoligono(poligono);
                //}



            Objeto objetoU = Objeto.CargarObjeto("U1");

            //Objeto objetoU = new Objeto();

            //objetoU.SetCentroDeMasa(new Punto(2f, 2f, 0.0f));
            //objetoU.Addparte(partesU);
            //objetoU.Rotar(x: 20f, y: 20f);
            //objetoU.GuardarObjeto("U1");

            Objeto objetoU2 = Objeto.CargarObjeto("U2");
            //Objeto objetoU2 = new Objeto();
            //objetoU2.SetCentroDeMasa(new Punto(-2f, -2f, 0.0f));
            //objetoU2.Addparte(partesU);
            //objetoU2.GuardarObjeto("U2");

            //Objeto objetoU3 = new Objeto(new Punto(-2f, 2f, 0.0f));
            Objeto objetoU3 = Objeto.CargarObjeto("U3");
            //objetoU3.SetCentroDeMasa(new Punto(-2f, 2f, 0.0f));
            //objetoU3.Addparte(partesU);
            //objetoU3.GuardarObjeto("U3");

                //Objeto objetoU4 = new Objeto(new Punto(2f, -2f, 0.0f));
                Objeto objetoU4 = Objeto.CargarObjeto("U4");
            //objetoU4.SetCentroDeMasa(new Punto(2f, -2f, 0.0f));
            //objetoU4.Addparte(partesU);
            //objetoU4.GuardarObjeto("U4");

                // Crear el escenario y agregar objetos
                escenario = new Escenario();
                escenario.AddObjeto("U",objetoU);
                escenario.AddObjeto("U2", objetoU2);
                escenario.AddObjeto("U3", objetoU3);
                escenario.AddObjeto("U4", objetoU4);
                // Guardar el objeto en un archivo .obj
                //GuardarObj(puntosU);

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
