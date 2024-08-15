using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_ReporteFalla_Componente
    {
        public int id { get; set; }
        public int idReporteFalla { get; set; }
        public int Conjunto { get; set; }
        public int Subconjunto { get; set; }
        public int Componente { get; set; }
        public bool AplicaOH { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Horas { get; set; }
        public string Parte { get; set; }
        public string cc { get; set; }
        [ForeignKey("idReporteFalla")]
        public virtual tblM_ReporteFalla Reporte { get; set; }
    }
}
