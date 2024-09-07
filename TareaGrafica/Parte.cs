using OpenTK.Graphics.OpenGL;

namespace TareaGrafica
{
    internal class Parte
    {
        private List<Poligono> listaPoligonos;
        private Punto centroMasa;

        public Parte()
        {
            listaPoligonos = new List<Poligono>(); 
            centroMasa = new Punto(0.0f, 0.0f, 0.0f);
        }

        public void AddPoligono(Poligono poligono)
        {
            listaPoligonos.Add(poligono);
        }

        public void Dibujar() 
        {

            GL.PushMatrix();
            GL.Translate(centroMasa.X, centroMasa.Y, centroMasa.Z);
            foreach (var poligono in listaPoligonos)
            {
                poligono.Dibujar();
            }

            GL.PopMatrix();
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
