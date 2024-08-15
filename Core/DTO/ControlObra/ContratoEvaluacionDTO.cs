using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.ControlObra;

namespace Core.DTO.ControlObra
{
    public class ContratoEvaluacionDTO
    {
        public int contrato_id { get; set; }
        public int asignacion_id { get; set; }
        public int evaluacion_id { get; set; }
        public string numeroContrato { get; set; }
        public string cc { get; set; }
        public string ccDesc { get; set; }
        public int subcontratista_id { get; set; }
        public string subcontratistaDesc { get; set; }
        public string periodoEvaluable { get; set; }
        public DateTime? fechaEvaluacion { get; set; }
        public string fechaEvaluacionString { get; set; }
        public bool flagCargaSoportesEnTiempo { get; set; }
        public string cargaSoportes { get; set; }
        public DateTime? fechaCargaSoporte { get; set; }
        public EstatusEvaluacionEnum estatusEvaluacion { get; set; }
        public string estatusFirmas { get; set; }

        public int evidencia_id { get; set; }
        public string comentarioEvaluacion { get; set; }
        public string planAccion { get; set; }
        public string responsable { get; set; }
        public DateTime? fechaCompromiso { get; set; }
        public string fechaCompromisoString { get; set; }
        public decimal calificacion { get; set; }
        public int ponderacion { get; set; }
        public EstatusEvidenciaEnum estatusEvidencia { get; set; }
        public bool evaluacionFirmada { get; set; }
        public int cambioEvaluacion_id { get; set; }
        public bool flagGestionFirmas { get; set; }
    }
}
