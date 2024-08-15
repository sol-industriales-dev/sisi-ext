using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.ControlObra.Dashboard
{
    public class GraficaCumplimientoEvaluacionesDTO
    {
        public int id_evaluacion { get; set; }
        public int id_contrato { get; set; }
        public int contratoCompromisoAutorizadas { get; set; }
        public int contratoCompromismoRequeridas { get; set; }
        public int contratoTiempoAutorizadas { get; set; }
        public int contratoTiempoRequeridas { get; set; }
        public int id_elemento { get; set; }
        public int id_plantilla { get; set; }
        public int totalAutorizadas { get; set; }
        public int totalRequeridas { get; set; }
        public decimal porcAutorizado { get; set; }
        public string descElemento { get; set; }
    }
}
