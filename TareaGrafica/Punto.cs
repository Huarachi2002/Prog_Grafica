using System.Globalization;
using System.Text.Json.Serialization;

namespace TareaGrafica
{
    internal class Punto
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        [JsonConstructor]
        public Punto()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public Punto(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"{X.ToString(CultureInfo.CreateSpecificCulture("es-ES"))} {Y.ToString(CultureInfo.CreateSpecificCulture("es-ES"))} {Z.ToString(CultureInfo.CreateSpecificCulture("es-ES"))}";
        }
    }
}
