﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TareaGrafica
{
    internal class Escenario
    {
        private List<Objeto> listaDeObjetos;

        public Escenario()
        {
            listaDeObjetos = new List<Objeto>();
        }

        public void AddObjeto(Objeto objeto)
        {
            listaDeObjetos.Add(objeto);
        }

        public void DibujarEscenario()
        {
            foreach (var objeto in listaDeObjetos)
            {
                objeto.Dibujar();
            }
        }

        public List<Objeto> GetObjetos()
        {
            return listaDeObjetos;
        }
    }
}
