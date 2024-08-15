using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class AuditorPrivilegioDTO
    {
        #region VERSION ANTERIOR
        //public int id { get; set; }
        //public string auditor { get; set; }
        //public string puesto { get; set; }
        //public int? privilegioId { get; set; }
        #endregion

        public int id { get; set; }
        public string auditor { get; set; }
        public string puesto { get; set; }
        public List<int> lstPrivilegiosID { get; set; }
    }
}
