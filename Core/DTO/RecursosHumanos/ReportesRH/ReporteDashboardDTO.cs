using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.ReportesRH
{
    public class ReporteDashboardDTO
    {
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int cantidad { get; set; }
        public int? cantAltas { get; set; }
        public int? cantBajas { get; set; }
        public decimal? porcAltas { get; set; }
        public decimal? porcBajas { get; set; }
    }
}
