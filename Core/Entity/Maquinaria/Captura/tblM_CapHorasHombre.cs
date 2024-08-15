using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CapHorasHombre
    {
        public int id { get; set; }
        public string centroCostos { get; set; }
        public string nombreEmpleado { get; set; }
        public int numEmpleado { get; set; }
        public int categoriaTrabajo { get; set; }
        public int subCategoria { get; set; }
        public decimal tiempo { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int usuarioCapturaID { get; set; }
        public int  turno { get; set; }
        public int puestoID { get; set; }
    }
}
