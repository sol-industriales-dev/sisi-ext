using Core.Enum.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Overhaul
{
    public class tblM_ReporteFalla_Archivo
    {
        public int id { get; set; }
        public int idReporteFalla { get; set; }
        public rptFallaTipoArchivoEnum tipo { get; set; }
        public string nombre { get; set; }
        public string ruta { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        [ForeignKey("idReporteFalla")]
        public virtual tblM_ReporteFalla rptFalla { get; set; }
    }
}
