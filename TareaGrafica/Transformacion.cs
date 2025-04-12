using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TareaGrafica
{
    internal class Transformacion
    {
        public Punto Traslacion { get; set; }
        public float RotacionX { get; set; }
        public float RotacionY { get; set; }
        public float RotacionZ { get; set; }
        public float EscalaX { get; set; }
        public float EscalaY { get; set; }
        public float EscalaZ { get; set; }

        public Transformacion()
        {
            Traslacion = new Punto(0, 0, 0);
            RotacionX = 0;
            RotacionY = 0;
            RotacionZ = 0;
            EscalaX = 1;
            EscalaY = 1;
            EscalaZ = 1;
        }

        // Método para combinar transformaciones (por ejemplo, de objeto y parte)
        public Transformacion Combinar(Transformacion otra)
        {
            Transformacion resultado = new Transformacion();

            // Suma las traslaciones
            resultado.Traslacion = new Punto(
                this.Traslacion.X + otra.Traslacion.X,
                this.Traslacion.Y + otra.Traslacion.Y,
                this.Traslacion.Z + otra.Traslacion.Z
            );

            // Suma las rotaciones
            resultado.RotacionX = this.RotacionX + otra.RotacionX;
            resultado.RotacionY = this.RotacionY + otra.RotacionY;
            resultado.RotacionZ = this.RotacionZ + otra.RotacionZ;

            // Multiplica las escalas
            resultado.EscalaX = this.EscalaX * otra.EscalaX;
            resultado.EscalaY = this.EscalaY * otra.EscalaY;
            resultado.EscalaZ = this.EscalaZ * otra.EscalaZ;

            return resultado;
        }
    }
}
