using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_FormatoCambioExclusion
    {
        #region SQL
        public int id { get; set; }
        public int empleadoCVE { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
