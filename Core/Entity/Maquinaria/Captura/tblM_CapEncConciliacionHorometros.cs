using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CapEncConciliacionHorometros
    {

        public int id { get; set; }
        public int centroCostosID { get; set; }
        public int fechaID { get; set; }
        public DateTime FechaCaptura { get; set; }
        public int usuarioCaptura { get; set; }
        public int estatus { get; set; }
        public bool esQuincena { get; set; }
        public int anio { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public bool facturado { get; set; }
        public string factura { get; set; }
    }
}
