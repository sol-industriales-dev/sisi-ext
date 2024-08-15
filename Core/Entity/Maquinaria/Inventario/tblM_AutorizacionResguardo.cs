using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_AutorizacionResguardo
    {
        public int id { get; set; }
        public int ResguardoVehiculoID { get; set; }
        public int usuarioElaboroID { get; set; }
        public string usuarioElaboroFirma { get; set; }
        public int usuarioReguardoIDEK { get; set; }
        public int usuarioSeguridadID { get; set; }
        public string usuarioSeguridadFirma { get; set; }
        public bool estatus { get; set; }
        public DateTime fechaRegistro { get; set; }

    }
}
