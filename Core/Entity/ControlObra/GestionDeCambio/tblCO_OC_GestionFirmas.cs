using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.ControlObra;

namespace Core.Entity.ControlObra.GestionDeCambio
{
    public class tblCO_OC_GestionFirmas
    {
        public int id { get; set; }
        public int idEmpleado { get; set; }
        public string cc { get; set; }
        public PrivilegioOrdenCambioEnum privilegio { get; set; }
        public bool estatus { get; set; }
    }
}
