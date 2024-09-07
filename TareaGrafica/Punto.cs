using System.Globalization;

namespace TareaGrafica
{
    internal class Punto
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

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
