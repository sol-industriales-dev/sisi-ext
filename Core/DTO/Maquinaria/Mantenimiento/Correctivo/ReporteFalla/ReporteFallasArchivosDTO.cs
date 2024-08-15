using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento.Correctivo.ReporteFalla
{
    public class ReporteFallasArchivosDTO
    {
        public int id { get; set; }
        public int idReporteFalla { get; set; }
        public int tipo { get; set; }
        public string strTipo { get; set; }
        public string nombre { get; set; }
        public string ruta { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
