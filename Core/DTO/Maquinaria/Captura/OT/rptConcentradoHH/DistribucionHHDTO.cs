using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT.rptConcentradoHH
{
    public class DistribucionHHDTO
    {
        public int puestoID { get; set; }
        public string puesto { get; set; }
        public decimal trabajoMaquinariaOT { get; set; }
        public decimal trabajosInstalaciones { get; set; }
        public decimal limpieza { get; set; }
        public decimal consultaInformacion { get; set; }
        public decimal tiempoDescanso { get; set; }
        public decimal cursosCapacitaciones { get; set; }
        public decimal monitoreoDiario { get; set; }
        public decimal totalHorashombre { get; set; }
    }
}
