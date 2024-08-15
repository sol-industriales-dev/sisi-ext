using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra.GestionDeCambio
{
    public class tblCO_OC_RelClavesEmpleadosEntreBD
    {
        public int id { get; set; }
        public int idUsuarioSubcontratista { get; set; }
        public int idUsuarioSigoplan { get; set; }
        public string claveEmpleadoEncontrol { get; set; }

    }
}
