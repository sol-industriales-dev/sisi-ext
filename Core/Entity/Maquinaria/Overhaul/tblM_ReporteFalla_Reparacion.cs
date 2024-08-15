using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_ReporteFalla_Reparacion
    {
        public int id { get; set; }
        public int idReporteFalla { get; set; }
        public int Tipo { get; set; }
        public int Grupo { get; set; }
        public int Insumo { get; set; }
        public string Bitacora { get; set; }
        [ForeignKey("idReporteFalla")]
        public virtual tblM_ReporteFalla Reporte { get; set; }
    }
}
