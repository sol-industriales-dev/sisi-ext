using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_CatSuministros
    {
      /// <summary>
      /// 11/05/18
      /// raguilar
      /// tipo:hace referencia a si es un siministro tipo lubricante, refrigerante
      /// sistema: si es para sistema gasolina, diesel o ambos
      /// 1.-diesel
      /// 2.-gasolina
      /// 3.-ambos
      /// </summary>
        public int id { get; set; }
        public string nomeclatura { get; set; }
        public bool estado { get; set; }
        public string  descripcion { get; set; }
        public int tipo { get; set; }
        public int sistema { get; set; }
    }
}
