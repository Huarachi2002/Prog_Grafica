using System.Runtime.CompilerServices;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using System.Diagnostics;
using OpenTK;

namespace TareaGrafica.UI
{
    public class ImGuiController : IDisposable
    {
        private bool _frameBegun;
        private int _vertexArray;
        private int _vertexBuffer;
        private int _indexBuffer;
        private int _fontTexture;
        private int _shader;
        private int _projectionMatrixLocation;
        private int _fontTextureLocation;
        private int _windowWidth;
        private int _windowHeight;

        // Datos del mouse y teclado
        private bool _leftMouseDown;
        private bool _rightMouseDown;
        private bool _middleMouseDown;
        private Vector2 _mousePosition;
        private Vector2 _mouseScroll;
        private bool _ctrlDown;
        private bool _altDown;
        private bool _shiftDown;

        // Lista de caracteres tecleados entre frames
        private List<char> _pressedChars = new List<char>();

        public ImGuiController(int width, int height)
        {
            _windowWidth = width;
            _windowHeight = height;

            IntPtr context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);

            ImGuiIOPtr io = ImGui.GetIO();
            io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;

            CreateDeviceObjects();

            SetPerFrameImGuiData(1f / 60f);
            ImGui.NewFrame();
            _frameBegun = true;
        }

        public void Update(float deltaSeconds)
        {
            if (_frameBegun)
            {
                ImGui.Render();
                _frameBegun = false;
            }

            SetPerFrameImGuiData(deltaSeconds);
            UpdateImGuiInput();

            _frameBegun = true;
            ImGui.NewFrame();
        }

        public void Render()
        {
            if (_frameBegun)
            {
                _frameBegun = false;
                ImGui.Render();
                RenderImDrawData(ImGui.GetDrawData());
            }
        }

        public void WindowResized(int width, int height)
        {
            _windowWidth = width;
            _windowHeight = height;
        }

        public void PressChar(char keyChar)
        {
            _pressedChars.Add(keyChar);
        }

        public void SetMouseDown(bool left, bool right, bool middle)
        {
            _leftMouseDown = left;
            _rightMouseDown = right;
            _middleMouseDown = middle;
        }

        public void SetMousePosition(float x, float y)
        {
            _mousePosition.X = x;
            _mousePosition.Y = y;
        }

        public void SetMouseScroll(float scrollX, float scrollY)
        {
            _mouseScroll.X = scrollX;
            _mouseScroll.Y = scrollY;
        }

        public void SetKeyModifiers(bool ctrl, bool alt, bool shift)
        {
            _ctrlDown = ctrl;
            _altDown = alt;
            _shiftDown = shift;
        }

        private void UpdateImGuiInput()
        {
            ImGuiIOPtr io = ImGui.GetIO();

            // Actualizar estado del mouse
            io.MouseDown[0] = _leftMouseDown;
            io.MouseDown[1] = _rightMouseDown;
            io.MouseDown[2] = _middleMouseDown;
            io.MousePos = new System.Numerics.Vector2(_mousePosition.X, _mousePosition.Y);
            io.MouseWheel = _mouseScroll.Y;
            io.MouseWheelH = _mouseScroll.X;

            // Resetear scroll después de usarlo
            _mouseScroll = Vector2.Zero;

            // Actualizar teclas modificadoras
            io.KeyCtrl = _ctrlDown;
            io.KeyAlt = _altDown;
            io.KeyShift = _shiftDown;

            // Procesar caracteres tecleados
            foreach (var c in _pressedChars)
            {
                io.AddInputCharacter(c);
            }
            _pressedChars.Clear();
        }

        private void SetPerFrameImGuiData(float deltaSeconds)
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.DisplaySize = new System.Numerics.Vector2(_windowWidth, _windowHeight);
            io.DisplayFramebufferScale = new System.Numerics.Vector2(1f, 1f);
            io.DeltaTime = deltaSeconds;
        }

        private void CreateDeviceObjects()
        {
            // Crear vertex array
            _vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArray);

            // Crear buffers
            _vertexBuffer = GL.GenBuffer();
            _indexBuffer = GL.GenBuffer();

            RecreateFontDeviceTexture();

            // Crear shader
            string vertexSource = @"
#version 330 core
uniform mat4 projection_matrix;
layout(location = 0) in vec2 position;
layout(location = 1) in vec2 uv;
layout(location = 2) in vec4 color;
out vec4 frag_color;
out vec2 frag_uv;
void main()
{
    frag_color = color;
    frag_uv = uv;
    gl_Position = projection_matrix * vec4(position.xy, 0, 1);
}";
            string fragmentSource = @"
#version 330 core
uniform sampler2D font_texture;
in vec4 frag_color;
in vec2 frag_uv;
out vec4 out_color;
void main()
{
    out_color = frag_color * texture(font_texture, frag_uv);
}";

            _shader = CreateShaderProgram(vertexSource, fragmentSource);
            _projectionMatrixLocation = GL.GetUniformLocation(_shader, "projection_matrix");
            _fontTextureLocation = GL.GetUniformLocation(_shader, "font_texture");
        }

        private void RecreateFontDeviceTexture()
        {
            ImGuiIOPtr io = ImGui.GetIO();

            // Eliminar textura existente si hay
            if (_fontTexture > 0)
            {
                GL.DeleteTexture(_fontTexture);
                _fontTexture = 0;
            }

            // Obtener datos de textura de ImGui
            io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out int bytesPerPixel);

            // Crear nueva textura
            _fontTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _fontTexture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

            // Guardar ID de textura en ImGui
            io.Fonts.SetTexID((IntPtr)_fontTexture);
            io.Fonts.ClearTexData();
        }

        private int CreateShaderProgram(string vertexSource, string fragmentSource)
        {
            // Compilar shaders
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexSource);
            GL.CompileShader(vertexShader);
            CheckShaderCompilation(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentSource);
            GL.CompileShader(fragmentShader);
            CheckShaderCompilation(fragmentShader);

            // Enlazar programa
            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);
            CheckProgramLinking(program);

            // Limpiar
            GL.DetachShader(program, vertexShader);
            GL.DetachShader(program, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            return program;
        }

        private void CheckShaderCompilation(int shader)
        {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
            if (status != 1)
            {
                string info = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error compilando shader: {info}");
            }
        }

        private void CheckProgramLinking(int program)
        {
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int status);
            if (status != 1)
            {
                string info = GL.GetProgramInfoLog(program);
                throw new Exception($"Error enlazando programa: {info}");
            }
        }

        private void RenderImDrawData(ImDrawDataPtr drawData)
        {
            //if (drawData.CmdListsCount == 0) return;

            //// Guardar estado de OpenGL
            //bool blendEnabled = GL.IsEnabled(EnableCap.Blend);
            //bool cullFaceEnabled = GL.IsEnabled(EnableCap.CullFace);
            //bool depthTestEnabled = GL.IsEnabled(EnableCap.DepthTest);
            //bool scissorTestEnabled = GL.IsEnabled(EnableCap.ScissorTest);

            //int lastActiveTexture = GL.GetInteger(GetPName.ActiveTexture);
            //int lastProgram = GL.GetInteger(GetPName.CurrentProgram);
            //int lastTexture = GL.GetInteger(GetPName.TextureBinding2D);
            //int lastArrayBuffer = GL.GetInteger(GetPName.ArrayBufferBinding);
            //int lastVertexArray = GL.GetInteger(GetPName.VertexArrayBinding);

            //// Guardar matrices
            //Matrix4 lastProjection = new Matrix4();
            //Matrix4 lastModelview = new Matrix4();
            //GL.GetFloat(GetPName.Modelview0MatrixExt, out lastProjection);
            //GL.GetFloat(GetPName.Modelview0MatrixExt, out lastModelview);

            //// Configurar estado OpenGL para ImGui
            //GL.Enable(EnableCap.Blend);
            //GL.BlendEquation(BlendEquationMode.FuncAdd);
            //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            //GL.Disable(EnableCap.CullFace);
            //GL.Disable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.ScissorTest);

            //// Configurar matrices
            //GL.Viewport(0, 0, _windowWidth, _windowHeight);
            //Matrix4 ortho = Matrix4.CreateOrthographicOffCenter(0, _windowWidth, _windowHeight, 0, -1, 1);
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadMatrix(ref ortho);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadIdentity();

            //// Usar shader
            //GL.UseProgram(_shader);
            //GL.UniformMatrix4(_projectionMatrixLocation, false, ref ortho);
            //GL.Uniform1(_fontTextureLocation, 0);

            //// Resto del código de renderizado...

            //// --- EL CÓDIGO DE RENDERIZADO ESPECÍFICO SIGUE AQUÍ ---

            //// DESPUÉS del renderizado, restaurar estado
            //GL.UseProgram(lastProgram);
            //GL.BindTexture(TextureTarget.Texture2D, lastTexture);
            //GL.ActiveTexture((TextureUnit)lastActiveTexture);
            //GL.BindVertexArray(lastVertexArray);
            //GL.BindBuffer(BufferTarget.ArrayBuffer, lastArrayBuffer);

            //// Restaurar matrices
            //GL.MatrixMode(MatrixMode.Projection);
            //GL.LoadMatrix(ref lastProjection);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadMatrix(ref lastModelview);

            //// Restaurar capacidades
            //if (!blendEnabled) GL.Disable(EnableCap.Blend);
            //if (cullFaceEnabled) GL.Enable(EnableCap.CullFace);
            //if (depthTestEnabled) GL.Enable(EnableCap.DepthTest);
            //if (!scissorTestEnabled) GL.Disable(EnableCap.ScissorTest);

        }

        public void Dispose()
        {
            GL.DeleteVertexArray(_vertexArray);
            GL.DeleteBuffer(_vertexBuffer);
            GL.DeleteBuffer(_indexBuffer);
            GL.DeleteTexture(_fontTexture);
            GL.DeleteProgram(_shader);

            ImGui.DestroyContext();
        }
    }
}
