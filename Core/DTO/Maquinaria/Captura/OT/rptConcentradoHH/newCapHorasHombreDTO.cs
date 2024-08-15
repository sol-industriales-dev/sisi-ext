using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT.rptConcentradoHH
{
    public class newCapHorasHombreDTO
    {

        public int puestoID { get; set; }
        public string nombreEmpleado { get; set; }
        public int numEmpleado { get; set; }

        public List<tblM_CapHorasHombre> trabajosInstalacion { get; set; }
        public List<tblM_CapHorasHombre> limpieza { get; set; }
        public List<tblM_CapHorasHombre> consultaInformacion { get; set; }
        public List<tblM_CapHorasHombre> tiempoDescanso { get; set; }
        public List<tblM_CapHorasHombre> cursoCapacitacion { get; set; }
        public List<tblM_CapHorasHombre> monitoreoDiario { get; set; }

        public decimal total { get; set; }

        public newCapHorasHombreDTO()
        {
            trabajosInstalacion = new List<tblM_CapHorasHombre>();
            limpieza = new List<tblM_CapHorasHombre>();
            consultaInformacion = new List<tblM_CapHorasHombre>();
            tiempoDescanso = new List<tblM_CapHorasHombre>();
            cursoCapacitacion = new List<tblM_CapHorasHombre>();
            monitoreoDiario = new List<tblM_CapHorasHombre>();
        }
    }
}
