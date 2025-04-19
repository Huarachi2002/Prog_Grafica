using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Text.Json.Serialization;

namespace TareaGrafica
{
    internal class Poligono
    {
        public List<Punto> puntos { get; set; }
        public float[] color { get; set; }

        [JsonConstructor]
        public Poligono()
        {
            puntos = new List<Punto>();
            color = new float[] { 1.0f, 1.0f, 1.0f };
        }

        public Poligono(List<Punto> puntos, float r, float g, float b)
        {
            this.puntos = puntos;
            this.color = new float[] { r, g, b };
        }


        public void Dibujar(Transformacion transformacion)
        {
            if (puntos == null || color == null)
            {
                Console.WriteLine("Error: Datos nulos en Poligono.Dibujar()");
                return;
            }

            // Aplicar todas las transformaciones recibidas
            GL.PushMatrix();
            
            // Aplicar escalas (si se implementa)
            GL.Scale(transformacion.EscalaX, transformacion.EscalaY, transformacion.EscalaZ);

            // Aplicar rotaciones
            GL.Rotate(transformacion.RotacionX, 1f, 0f, 0f);
            GL.Rotate(transformacion.RotacionY, 0f, 1f, 0f);
            GL.Rotate(transformacion.RotacionZ, 0f, 0f, 1f);
            
            // Aplicar traslación
            GL.Translate(
                transformacion.TraslacionX,
                transformacion.TraslacionY,
                transformacion.TraslacionZ
            );

            // Dibujar el polígono
            GL.Begin(PrimitiveType.Polygon);
            GL.Color3(color[0], color[1], color[2]);

            foreach (var punto in puntos)
            {
                GL.Vertex3(punto.X, punto.Y, punto.Z);
            }

            GL.End();

            GL.PopMatrix();
        }


        public static void PushMatrix()
        {
            GL.PushMatrix();
        }

        public static void PopMatrix()
        {
            GL.PopMatrix();
        }

        public static void Translate(float x, float y, float z)
        {
            GL.Translate(x, y, z);
        }

        public static void Rotate(float angulo, float x, float y, float z)
        {
            GL.Rotate(angulo, x, y, z);
        }


        public List<Punto> GetPuntos()
        {
            return puntos;
        }
    }
}
