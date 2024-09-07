using OpenTK.Graphics.OpenGL;

namespace TareaGrafica
{
    internal class Objeto
    {
        private List<Parte> listaPartes;
        private Punto centroDeMasa;


        public Objeto()
        {
            listaPartes = new List<Parte>();
            centroDeMasa = new Punto(0.0f, 0.0f, 0.0f); // Inicialmente en el origen
        }

        public void Addparte(Parte parte)
        {
            listaPartes.Add(parte);
        }

        public void Dibujar()
        {
            // Aplicar la traslación al centro de masa antes de dibujar cada polígono
            GL.PushMatrix();
            GL.Translate(centroDeMasa.X, centroDeMasa.Y, centroDeMasa.Z);

            foreach (var parte in listaPartes)
            {
                parte.Dibujar();
            }

            GL.PopMatrix();
        }
        public void SetCentroDeMasa(Punto nuevoCentro)
        {
            centroDeMasa = nuevoCentro;
        }
        public List<Parte> Getpartes()
        {
            return listaPartes;
        }
    }
}
