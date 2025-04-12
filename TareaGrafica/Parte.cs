using OpenTK.Graphics.OpenGL;
using System.Text.Json.Serialization;

namespace TareaGrafica
{
    internal class Parte
    {
        public List<Poligono> listaPoligonos { get; set; }

        public Punto centroMasa { get; set; }
        public float rotacionX { get; set; }
        public float rotacionY { get; set; }
        public float rotacionZ { get; set; }

        [JsonConstructor]
        public Parte()
        {
            listaPoligonos = new List<Poligono>();
            centroMasa = new Punto(0.0f, 0.0f, 0.0f);
        }

        public Parte(Punto? centroMasa = null)
        {
            listaPoligonos = new List<Poligono>();
            centroMasa = centroMasa ?? new Punto(0.0f, 0.0f, 0.0f);
        }

        public void AddPoligono(Poligono poligono)
        {
            listaPoligonos.Add(poligono);
        }

        public void RemovePoligono(Poligono poligono)
        {
            listaPoligonos.Remove(poligono);
        }

        public void Dibujar() 
        {
            GL.PushMatrix();
            GL.Translate(centroMasa.X, centroMasa.Y, centroMasa.Z);
            GL.Rotate(rotacionX, 1f, 0f, 0f);
            GL.Rotate(rotacionY, 0f, 1f, 0f);
            GL.Rotate(rotacionZ, 0f, 0f, 1f);
            if (listaPoligonos == null)
            {
                Console.WriteLine("Error: listaPoligonos es null");
                return;
            }

            foreach (var poligono in listaPoligonos)
            {
                poligono.Dibujar();
            }

            GL.PopMatrix();
        }

        public void Rotar(float? x = null, float? y = null, float? z = null)
        {
            if (x.HasValue)
                rotacionX += x.Value;

            if (y.HasValue)
                rotacionY += y.Value;

            if (z.HasValue)
                rotacionZ += z.Value;
        }


        public void SetCentroMasa(Punto newCentroMasa)
        {
            centroMasa = newCentroMasa;
        }

        public List<Poligono> GetPoligonos()
        {
            return listaPoligonos;
        }
    }
}
