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
            //this.primitiveType.LineLoop
        }


        public void Dibujar()
        {
            if (puntos == null || color == null)
            {
                Console.WriteLine("Error: Datos nulos en Poligono.Dibujar()");
                return;
            }
            GL.Begin(PrimitiveType.Polygon); // Cambiado de Polygon a Quads
            GL.Color3(color[0], color[1], color[2]);

            foreach (var punto in puntos)
            {
                GL.Vertex3(punto.X, punto.Y, punto.Z);
            }

            GL.End();
        }

        public List<Punto> GetPuntos()
        {
            return puntos;
        }
    }
}
