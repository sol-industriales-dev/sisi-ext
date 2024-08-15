using Core.DTO.Subcontratistas.Evaluacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.ControlObra.EvaluacionSubcontratista;

namespace Core.DTO.ControlObra
{
    public class ReporteEvaluacionSubcontratistaDTO
    {
        public string fechaEvaluacion { get; set; }
        public string periodoEvaluacion { get; set; }
        public string periodoEjecucion { get; set; }
        public string subcontratistaNombre { get; set; }
        public string numeroContrato { get; set; }
        public string servicioContratado { get; set; }
        public string proyectoNombre { get; set; }
        public string evaluadorNombre { get; set; }
        public string calificacionEvaluacion { get; set; }
        public List<DivReqDTO> listaElementosRequerimientos { get; set; }
        public List<reporteCristal> listaRetroalimentacion { get; set; }
    }
}
