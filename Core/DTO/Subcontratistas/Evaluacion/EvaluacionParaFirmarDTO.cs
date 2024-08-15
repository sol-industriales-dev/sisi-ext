using Core.Entity.SubContratistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Subcontratistas.Evaluacion
{
    public class EvaluacionParaFirmarDTO
    {
        public int evaluacionId { get; set; }
        public int contratoId { get; set; }
        public string numeroContrato { get; set; }
        public int subcontratistaId { get; set; }
        public string nombreSubcontratista { get; set; }
        public string proyecto { get; set; }
        public string nombreProyecto { get; set; }
        public string nombreEvaluacion { get; set; }
        public bool firmado { get; set; }
        public bool elUsuarioPuedeFirmar { get; set; }
        public tblX_FirmaEvaluacion firma { get; set; }
    }
}
