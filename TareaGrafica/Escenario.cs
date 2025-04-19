namespace TareaGrafica
{
    internal class Escenario
    {
        public Dictionary<String, Objeto> listaDeObjetos { get; set; }
        public float rotacionX { get; set; }
        public float rotacionY { get; set; }
        public float rotacionZ { get; set; }
        public float escalaX { get; set; }
        public float escalaY { get; set; }
        public float escalaZ { get; set; }
        public float traslacionX { get; set; }
        public float traslacionY { get; set; }
        public float traslacionZ { get; set; }

        public Escenario()
        {
            listaDeObjetos = new Dictionary<string, Objeto>();
            traslacionX = traslacionY = traslacionZ = 0.0f;
            rotacionX = rotacionY = rotacionZ = 0.0f;
            escalaX = escalaY = escalaZ = 1.0f;
        }

        public void AddObjeto(String name, Objeto objeto)
        {
            listaDeObjetos.Add(name, objeto);
        }

        public void RemoveObjeto(String name)
        {
            listaDeObjetos.Remove(name);
        }

        public void DibujarEscenario()
        {
            if(listaDeObjetos == null)
            {
                Console.WriteLine("Error: datos nulos en Escenario.Dibujar()");
                return;
            }
            Transformacion transformacionEscenario = new Transformacion
            {
                TraslacionX = traslacionX,
                TraslacionY = traslacionY,
                TraslacionZ = traslacionZ,
                RotacionX = rotacionX,
                RotacionY = rotacionY,
                RotacionZ = rotacionZ,
                EscalaX = escalaX,
                EscalaY = escalaY,
                EscalaZ = escalaZ
            };

            foreach (var objeto in listaDeObjetos.Values)
            {
                objeto.Dibujar(transformacionEscenario);   
            }
        }

        public void Rotar(float? x = null, float? y = null, float? z = null)
        {
            if(x.HasValue)
                rotacionX = x.Value;

            if(y.HasValue)
                rotacionY = y.Value;

            if (z.HasValue)
                rotacionZ = z.Value;
        }

        public void Escalar(float? x = null, float? y = null, float? z = null)
        {
            if(x.HasValue)
                escalaX = x.Value;

            if(y.HasValue)
                escalaY = y.Value;

            if(z.HasValue)
                escalaZ = z.Value;
        }

        public void Trasladar(float? x = null, float? y = null, float? z = null)
        {
            if (x.HasValue)
                traslacionX = x.Value;

            if (y.HasValue)
                traslacionY = y.Value;

            if (z.HasValue)
                traslacionZ = z.Value;
        }

        public Dictionary<String, Objeto> GetObjetos()
        {
            return listaDeObjetos;
        }

        public Objeto GetObjetoByKey(String name)
        {
            return listaDeObjetos[name];
        }
    }
}
