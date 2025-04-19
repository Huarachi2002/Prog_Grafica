using OpenTK.Graphics.OpenGL;
using System.Text.Json.Serialization;

namespace TareaGrafica
{
    internal class Objeto
    {
        public Dictionary<String, Parte> listaPartes { get; set; }
        public Punto centroDeMasa { get; set; }
        public float rotacionX { get; set; }
        public float rotacionY { get; set; }
        public float rotacionZ { get; set; }
        public float escalaX { get; set; }
        public float escalaY { get; set; }
        public float escalaZ { get; set; }

        [JsonConstructor]
        public Objeto()
        {
            listaPartes = new Dictionary<String, Parte>();
            centroDeMasa = new Punto(0.0f, 0.0f, 0.0f);
            escalaX = escalaY = escalaZ = 1.0f;
            rotacionX = rotacionY = rotacionZ = 0.0f;
        }

        public Objeto(Punto? centroMasa = null)
        {
            listaPartes = new Dictionary<String, Parte>();
            centroDeMasa = centroMasa ?? new Punto(0.0f, 0.0f, 0.0f);
            rotacionX = rotacionY = rotacionZ = 0.0f;
            escalaX = escalaY = escalaZ = 1.0f;
        }

        public void Addparte(String name, Parte parte)
        {
            listaPartes.Add(name, parte);
        }

        public void Removeparte(String name)
        {
            listaPartes.Remove(name);
        }

        public void Dibujar(Transformacion transformacionEscenario)
        {
            if (centroDeMasa == null || listaPartes == null)
            {
                Console.WriteLine("Error: datos nulos en Objeto.Dibujar()");
                return;
            }

            Transformacion transformacionObjeto = new Transformacion
            {
                TraslacionX = centroDeMasa.X,
                TraslacionY = centroDeMasa.Y,
                TraslacionZ = centroDeMasa.Z,
                RotacionX = rotacionX,
                RotacionY = rotacionY,
                RotacionZ = rotacionZ,
                EscalaX = escalaX,
                EscalaY = escalaY,
                EscalaZ = escalaZ
            };

            Transformacion transformacionFinal = transformacionEscenario.Combinar(transformacionObjeto);

            foreach (var parte in listaPartes.Values)
            {
                parte.Dibujar(transformacionFinal);
            }
        }

        public void Rotar(float? x = null, float? y = null, float? z = null)
        {
            if (x.HasValue)
                rotacionX = x.Value;

            if (y.HasValue)
                rotacionY = y.Value;

            if (z.HasValue)
                rotacionZ = z.Value;
        }

        public void Escalar(float? x = null, float? y = null, float? z = null)
        {
            if (x.HasValue)
                escalaX = x.Value;

            if (y.HasValue)
                escalaY = y.Value;

            if (z.HasValue)
                escalaZ = z.Value;

        }

        public void SetCentroDeMasa(Punto nuevoCentro)
        {
            centroDeMasa = nuevoCentro;
        }
        public Dictionary<String, Parte> Getpartes()
        {
            return listaPartes;
        }

        public Parte GetParteByKey(String name)
        {
            return listaPartes[name];
        }

    }
}
