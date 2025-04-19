using ImGuiNET;
using OpenTK;
    using OpenTK.Graphics;
    using OpenTK.Graphics.OpenGL;
    using OpenTK.Input;
using TareaGrafica.UI;

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

        private ImGuiController imGuiController;
        private SideBar sidebar;

        // Variables para gestionar el estado de entrada
        private bool _leftMouseDown;
        private bool _rightMouseDown;
        private bool _middleMouseDown;
        private Vector2 _mousePosition;



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
                    0.1f, 
                    100.0f 
                );
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref projection);

                //List<Punto> puntosU = new List<Punto>
                //{
                //    new Punto(-1, 1,  0.5f), // 0
                //    new Punto( 1, 1,  0.5f), // 1
                //    new Punto( 1, -1, 0.5f), // 2
                //    new Punto(-1, -1, 0.5f), // 3
                //    new Punto(-1, 1, -0.5f), // 4
                //    new Punto( 1, 1, -0.5f), // 5
                //    new Punto( 1, -1, -0.5f), // 6
                //    new Punto(-1, -1, -0.5f), // 7
                //    new Punto(-0.7f, 1,  0.5f), // 8
                //    new Punto( 0.7f, 1,  0.5f), // 9
                //    new Punto( 0.7f, -0.7f, 0.5f), // 10
                //    new Punto(-0.7f, -0.7f, 0.5f), // 11
                //    new Punto(-0.7f, 1, -0.5f), // 12
                //    new Punto( 0.7f, 1, -0.5f), // 13
                //    new Punto( 0.7f, -0.7f, -0.5f), // 14
                //    new Punto(-0.7f, -0.7f, -0.5f)  // 15
                //};


                //List<Poligono> poligonosU = new List<Poligono>
                //{
                //    // Parte trasera del lado izq
                //    new Poligono(new List<Punto> { puntosU[4], puntosU[12], puntosU[15], puntosU[7] }, 1.0f, 0.0f, 0.0f),
                //    // Parte trasera inferior
                //    new Poligono(new List<Punto> { puntosU[7], puntosU[15], puntosU[14], puntosU[6] }, 1.0f, 0.0f, 0.0f),
                //    // Parte trasera del lado der
                //    new Poligono(new List<Punto> { puntosU[13], puntosU[5], puntosU[6], puntosU[14] }, 1.0f, 0.0f, 0.0f),
                //    // Parte delantera del lado izq
                //    new Poligono(new List<Punto> { puntosU[0], puntosU[8], puntosU[11], puntosU[3] }, 0.0f, 1.0f, 0.0f),
                //    // Parte delantera inferior
                //    new Poligono(new List<Punto> { puntosU[3], puntosU[11], puntosU[10], puntosU[2] }, 0.0f, 1.0f, 0.0f),
                //    // Parte delantera del lado der
                //    new Poligono(new List<Punto> { puntosU[9], puntosU[1], puntosU[2], puntosU[10] }, 0.0f, 1.0f, 0.0f),
                //    // Lado izquierdo
                //    new Poligono(new List<Punto> { puntosU[0], puntosU[4], puntosU[7], puntosU[3] }, 0.0f, 0.0f, 1.0f),
                //    // Lado izquierdo der
                //    new Poligono(new List<Punto> { puntosU[8], puntosU[12], puntosU[15], puntosU[11] }, 0.0f, 0.0f, 1.0f),
                //    // Lado derecho
                //    new Poligono(new List<Punto> { puntosU[1], puntosU[5], puntosU[6], puntosU[2] }, 1.0f, 1.0f, 0.0f),
                //    // Lado derecho izq
                //    new Poligono(new List<Punto> { puntosU[9], puntosU[13], puntosU[14], puntosU[10] }, 1.0f, 1.0f, 0.0f),
                //    // Parte inferior abajo
                //    new Poligono(new List<Punto> { puntosU[3], puntosU[7], puntosU[6], puntosU[2] }, 1.0f, 0.0f, 1.0f),
                //    // Parte inferior arriba
                //    new Poligono(new List<Punto> { puntosU[11], puntosU[15], puntosU[14], puntosU[10] }, 1.0f, 0.0f, 1.0f),
                //    // Parte superior izq
                //    new Poligono(new List<Punto> { puntosU[0], puntosU[8], puntosU[12], puntosU[4] }, 0.0f, 1.0f, 1.0f),
                //    // Parte superior der
                //    new Poligono(new List<Punto> { puntosU[9], puntosU[1], puntosU[5], puntosU[13] }, 0.0f, 1.0f, 1.0f)
                //};


                //Parte partesU = new Parte();
                //foreach (var poligono in poligonosU)
                //{
                //    partesU.AddPoligono(poligono);
                //}



            Objeto objetoU = JsonHelper.DeserializeFromJson<Objeto>("U1");

            //Objeto objetoU = new Objeto();
            //objetoU.SetCentroDeMasa(new Punto(2f, 2f, 0.0f));
            //objetoU.Addparte("parteU",partesU);
            //objetoU.Rotar(x: 20f, y: 20f);
            //objetoU.Escalar(1.5f, 1.5f, 1.5f);
            //JsonHelper.SerializeToJson(objetoU, "U1");

            Objeto objetoU2 = JsonHelper.DeserializeFromJson<Objeto>("U2");

            //Objeto objetoU2 = new Objeto();
            //objetoU2.SetCentroDeMasa(new Punto(-2f, -2f, 0.0f));
            //objetoU2.Addparte("parteU",partesU);
            //JsonHelper.SerializeToJson(objetoU2, "U2");

            Objeto objetoU3 = JsonHelper.DeserializeFromJson<Objeto>("U3");

            //Objeto objetoU3 = new Objeto(new Punto(-2f, 2f, 0.0f));
            //objetoU3.SetCentroDeMasa(new Punto(-2f, 2f, 0.0f));
            //objetoU3.Addparte("parteU",partesU);
            //JsonHelper.SerializeToJson(objetoU3, "U3");

            Objeto objetoU4 = JsonHelper.DeserializeFromJson<Objeto>("U4");
            objetoU4.SetCentroDeMasa(new Punto(3.0f, 0.0f, 0.0f));
            objetoU4.Escalar(0.5f, 1.0f, .5f);

            //Objeto objetoU4 = new Objeto(new Punto(2f, -2f, 0.0f));
            //objetoU4.SetCentroDeMasa(new Punto(2f, -2f, 0.0f));
            //objetoU4.Addparte("parteU", partesU);
            //JsonHelper.SerializeToJson(objetoU4, "U4");

            escenario = new Escenario();
                escenario.AddObjeto("U",objetoU);
                escenario.AddObjeto("U2", objetoU2);
                escenario.AddObjeto("U3", objetoU3);
                escenario.AddObjeto("U4", objetoU4);
                //escenario.Trasladar(1.5f, 0.0f, 0.0f);
                //escenario.Rotar(0.0f, 0.0f, 90.0f);

            JsonHelper.SerializeToJson(escenario, "Escenario");

            //imGuiController = new ImGuiController(Width, Height);

            //sidebar = new SideBar(escenario);
            //pitch = 15.0f;  // Inclinación inicial
            //yaw = 45.0f;    // Rotación inicial
            //zoom = 10.0f;   // Distancia inicial
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
                GL.Rotate(pitch, 1.0f, 0.0f, 0.0f);
                GL.Rotate(yaw, 0.0f, 1.0f, 0.0f);   

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

            // Actualizar ImGui
            //imGuiController.Update((float)e.Time);
            //sidebar.Render();
            //imGuiController.Render();
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

            //if (e.Button == MouseButton.Left)
            //{
            //    _leftMouseDown = true;
            //    lastMousePos = new Vector2(e.X, e.Y);
            //}
            //else if (e.Button == MouseButton.Right)
            //    _rightMouseDown = true;
            //else if (e.Button == MouseButton.Middle)
            //    _middleMouseDown = true;

            //ImGuiIOPtr io = ImGui.GetIO();
            //// Solo enviar eventos de mouse a ImGui si están sobre la interfaz
            //if (io.WantCaptureMouse)
            //{
            //    imGuiController.SetMouseDown(_leftMouseDown, _rightMouseDown, _middleMouseDown);
            //}
        }

            protected override void OnMouseUp(MouseButtonEventArgs e)
            {
                base.OnMouseUp(e);

            if (e.Button == MouseButton.Left)
            {
                isMouseDown = false;
            }

            //if (e.Button == MouseButton.Left)
            //    _leftMouseDown = false;
            //else if (e.Button == MouseButton.Right)
            //    _rightMouseDown = false;
            //else if (e.Button == MouseButton.Middle)
            //    _middleMouseDown = false;

            //imGuiController.SetMouseDown(_leftMouseDown, _rightMouseDown, _middleMouseDown);
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

                //imGuiController.SetMouseScroll(e.X, e.Y);
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
            //_mousePosition = new Vector2(e.X, e.Y);
            //imGuiController.SetMousePosition(e.X, e.Y);
            //ImGuiIOPtr io = ImGui.GetIO();
            //if (_leftMouseDown && !io.WantCaptureMouse)
            //{
            //    Vector2 delta = new Vector2(e.X, e.Y) - lastMousePos;
            //    lastMousePos = new Vector2(e.X, e.Y);

            //    yaw += delta.X * 0.5f;
            //    pitch += delta.Y * 0.5f;
            //}

            //// Actualizar la última posición del mouse en cualquier caso
            //lastMousePos = new Vector2(e.X, e.Y);
        }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                GL.Viewport(0, 0, Width, Height); // Configurar el viewport al tamaño de la ventana
                //imGuiController.WindowResized(Width, Height);
        }

            protected override void OnUpdateFrame(FrameEventArgs e)
            {
                base.OnUpdateFrame(e);
                // Lógica de actualización, como manejar la entrada del usuario, se coloca aquí
            }
        //    protected override void OnKeyPress(KeyPressEventArgs e)
        //    {
        //        base.OnKeyPress(e);

        //        // Pasar eventos de texto a ImGui
        //        imGuiController.PressChar(e.KeyChar);
        //    }

        //    protected override void OnKeyDown(KeyboardKeyEventArgs e)
        //    {
        //        base.OnKeyDown(e);

        //        imGuiController.SetKeyModifiers(
        //            e.Control,
        //            e.Alt,
        //            e.Shift
        //        );
        //    }

        //protected override void OnKeyUp(KeyboardKeyEventArgs e)
        //{
        //    base.OnKeyUp(e);

        //    // Obtener el estado actual del teclado
        //    var keyboardState = Keyboard.GetState();

        //    // Configurar los modificadores de tecla en ImGuiController
        //    imGuiController.SetKeyModifiers(
        //        keyboardState.IsKeyDown(Key.LControl) || keyboardState.IsKeyDown(Key.RControl), // Control
        //        keyboardState.IsKeyDown(Key.LAlt) || keyboardState.IsKeyDown(Key.RAlt),       // Alt
        //        keyboardState.IsKeyDown(Key.LShift) || keyboardState.IsKeyDown(Key.RShift)    // Shift
        //    );
        //}

    }
    }
