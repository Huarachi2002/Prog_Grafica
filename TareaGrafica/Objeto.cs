using OpenTK.Graphics.OpenGL;
using System.Text.Json.Serialization;

namespace TareaGrafica
{
    internal class Objeto
    {
        public List<Parte> listaPartes { get; set; }
        public Punto centroDeMasa { get; set; }
        public float rotacionX { get; set; }
        public float rotacionY { get; set; }
        public float rotacionZ { get; set; }

        [JsonConstructor]
        public Objeto()
        {
            listaPartes = new List<Parte>();
            centroDeMasa = new Punto(0.0f, 0.0f, 0.0f);
        }

        public Objeto(Punto? centroMasa = null)
        {
            listaPartes = new List<Parte>();
            centroDeMasa = centroMasa ?? new Punto(0.0f, 0.0f, 0.0f);
            rotacionX = 0;
            rotacionY = 0;
            rotacionZ = 0;
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
            if (centroDeMasa == null || listaPartes == null)
            {
                Console.WriteLine("Error: datos nulos en Objeto.Dibujar()");
                return;
            }
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

        public bool GuardarObjeto(string rutaArchivo)
        {
            string carpetaDatos = Path.Combine(Directory.GetCurrentDirectory(), "Datos");
            Directory.CreateDirectory(carpetaDatos);
            string rutaCompleta = Path.Combine(carpetaDatos, $"{rutaArchivo}.json");
            bool resultado = JsonHelper.SerializeToJson(this, rutaCompleta);
            if (resultado)
            {
                Console.WriteLine($"Objeto guardado exitosamente en: {rutaCompleta}");
            }
            else
            {
                Console.WriteLine($"Error al guardar el objeto en: {rutaCompleta}");
            }

            return resultado;
        }

        public static Objeto CargarObjeto(string rutaArchivo)
        {
            string carpetaDatos = Path.Combine(Directory.GetCurrentDirectory(), "Datos");
            string rutaCompleta = Path.Combine(carpetaDatos, $"{rutaArchivo}.json");

            if (!File.Exists(rutaCompleta))
            {
                Console.WriteLine($"El archivo {rutaCompleta} no existe");
                return new Objeto();
            }

            try
            {
                Console.WriteLine($"Intentando cargar: {rutaCompleta}");
                Objeto objeto = JsonHelper.DeserializeFromJson<Objeto>(rutaCompleta);
                Console.WriteLine("Objeto cargado exitosamente");
                return objeto ?? new Objeto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando objeto: {ex.Message}");
                return new Objeto();
            }
        }
    }
}
