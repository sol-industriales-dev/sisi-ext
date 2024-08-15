using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.ActoCondicion
{
    public class MatrizDTO
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
        #endregion

        #region ADICIONAL
        public int noMatriz { get; set; }
        public string amonestacionDescripcion { get; set; }
        public string suspensionDescripcion { get; set; }
        public string recisionDescripcion { get; set; }
        public string sancionDescripcion { get; set; }
        public string clasificacion { get; set; }
        public string infraccion { get; set; }
        public string cc { get; set; }
        #endregion
    }
}
