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
        public float escalaX { get; set; }
        public float escalaY { get; set; }
        public float escalaZ { get; set; }

        [JsonConstructor]
        public Parte()
        {
            listaPoligonos = new List<Poligono>();
            centroMasa = new Punto(0.0f, 0.0f, 0.0f);
            escalaX = escalaY = escalaZ = 1.0f;
        }

        public Parte(Punto? centroMasa = null)
        {
            listaPoligonos = new List<Poligono>();
            this.centroMasa = centroMasa ?? new Punto(0.0f, 0.0f, 0.0f);
            escalaX = escalaY = escalaZ = 1.0f;
        }

        public void AddPoligono(Poligono poligono)
        {
            listaPoligonos.Add(poligono);
        }

        public void RemovePoligono(Poligono poligono)
        {
            listaPoligonos.Remove(poligono);
        }

        public void Dibujar(Transformacion transformacionObjeto)
        {
            if (listaPoligonos == null)
            {
                Console.WriteLine("Error: listaPoligonos es null");
                return;
            }

            Transformacion transformacionParte = new Transformacion
            {
                Traslacion = centroMasa,
                RotacionX = rotacionX,
                RotacionY = rotacionY,
                RotacionZ = rotacionZ,
                EscalaX = escalaX,
                EscalaY = escalaY,
                EscalaZ = escalaZ
            };

            Transformacion transformacionFinal = transformacionObjeto.Combinar(transformacionParte);

            foreach (var poligono in listaPoligonos)
            {
                poligono.Dibujar(transformacionFinal);
            }
        }

        public void Rotar(float? x = null, float? y = null, float? z = null)
        {
            if (x.HasValue) rotacionX += x.Value;
            if (y.HasValue) rotacionY += y.Value;
            if (z.HasValue) rotacionZ += z.Value;
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
