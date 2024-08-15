using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT.rptConcentradoHH
{
    public class CapDistribucionHHDTO
    {

        public int personalID { get; set; }
        public string empleado { get; set; }

        //public int catTrabajoMaquinariaOT { get; set; }
        //public int subCatTrabajoMaquinariaOT { get; set; }
        //public decimal valueTrabajoMaquinariaOT { get; set; }

        public int idcatTrabajosInstalaciones { get; set; }
        public int catTrabajosInstalaciones { get; set; }
        public int subCatTrabajosInstalaciones { get; set; }
        public decimal valueTrabajosInstalaciones { get; set; }
        public string descatTrabajosInstalaciones { get; set; }

        public int idcatLimpieza { get; set; }
        public int catLimpieza { get; set; }
        public int subCatLimpieza { get; set; }
        public decimal valueLimpieza { get; set; }
        public string descatLimpieza { get; set; }


        public int idcatConsultaInformacion { get; set; }
        public int catConsultaInformacion { get; set; }
        public int subCatConsultaInformacion { get; set; }
        public decimal valueConsultaInformacion { get; set; }
        public string descatConsultaInformacion { get; set; }

        public int idcatTiempoDescanso { get; set; }
        public int catTiempoDescanso { get; set; }
        public int subCatTiempoDescanso { get; set; }
        public decimal valueTiempoDescanso { get; set; }
        public string descatTiempoDescanso { get; set; }

        public int idcatCursosCapacitaciones { get; set; }
        public int catCursosCapacitaciones { get; set; }
        public int subCatCursosCapacitaciones { get; set; }
        public decimal valueCursosCapacitaciones { get; set; }
        public decimal valueTotalHorashombre { get; set; }
        public string descatCursosCapacitaciones { get; set; }

        public int idcatMonitoreoDiario { get; set; }
        public int catMonitoreoDiario { get; set; }
        public int subCatMonitoreoDiario { get; set; }
        public decimal valueMonitoreoDiario { get; set; }
        public decimal valueTotalMonitoreoDiario { get; set; }
        public string descatMonitoreoDiario { get; set; }

        public int puestoID { get; set; }
    }
}
