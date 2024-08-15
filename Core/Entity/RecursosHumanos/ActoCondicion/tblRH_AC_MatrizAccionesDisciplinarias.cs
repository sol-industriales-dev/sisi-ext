using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.ActoCondicion
{
    public class tblRH_AC_MatrizAccionesDisciplinarias
    {
        #region SQL
        public int id { get; set; }
        public int numero { get; set; }
        public string tipoInfraccion { get; set; }
        public int nivel { get; set; }
        public int amonestacion { get; set; }
        public int suspension { get; set; }
        public int rescision { get; set; }
        public int sancion { get; set; }
        public bool estatus { get; set; }
        public int FK_Clasificacion { get; set; }
        #endregion
    }
}
