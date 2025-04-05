using OpenTK.Graphics.OpenGL;

namespace TareaGrafica
{
    internal class Objeto
    {
        private List<Parte> listaPartes;
        private Punto centroDeMasa;
        private float rotacionX, rotacionY, rotacionZ;

        public Objeto(Punto? centroMasa = null)
        {
            listaPartes = new List<Parte>();
            centroDeMasa = centroMasa ?? new Punto(0.0f, 0.0f, 0.0f); // Inicialmente en el origen
        }

        public void Addparte(Parte parte)
        {
            listaPartes.Add(parte);
        }

        public void Removeparte(Parte parte)
        {
            listaPartes.Remove(parte);
        }

        public void Dibujar()
        {
            // Aplicar la traslación al centro de masa antes de dibujar cada polígono
            GL.PushMatrix();
            GL.Translate(centroDeMasa.X, centroDeMasa.Y, centroDeMasa.Z);
            GL.Rotate(rotacionX, 1f, 0f, 0f);
            GL.Rotate(rotacionY, 0f, 1f, 0f);
            GL.Rotate(rotacionZ, 0f, 0f, 1f);
            foreach (var parte in listaPartes)
            {
                parte.Dibujar();
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
