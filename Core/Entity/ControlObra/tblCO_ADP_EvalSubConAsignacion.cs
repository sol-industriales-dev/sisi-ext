using Core.Entity.SubContratistas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_ADP_EvalSubConAsignacion
    {
        public int id { get; set; }
        public int idContrato { get; set; }
        public int idPadre { get; set; }
        public string cc { get; set; }
        public int idSubContratista { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string firmaAutorizacion { get; set; }
        public DateTime? FechaAutorizacion { get; set; }
        public int statusAutorizacion { get; set; }
        public bool esActivo { get; set; }
        public int evaluacionAnteriorid { get; set; }
        public int evaluacionActual { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public DateTime fechaInicialEjecutable { get; set; }
        public DateTime fechaFinalEjecutable { get; set; }
        public int numFreq { get; set; }
        public bool statusVobo { get; set; }
        public int idPlantilla { get; set; }
        public string nombreEvaluacion { get; set; }
        public string servicioContratado { get; set; }

        [ForeignKey("idSubContratista")]
        public virtual tblX_SubContratista subcontratista { get; set; }

        [ForeignKey("idContrato")]
        public virtual tblX_Contrato contrato { get; set; }

        [ForeignKey("idSubConAsignacion")]
        public virtual List<tblCO_ADP_EvalSubContratista> evalSubContratista { get; set; }

        [ForeignKey("evaluacionId")]
        public virtual List<tblX_FirmaEvaluacion> firma { get; set; }

        public int idEstado { get; set; }
        public int idMunicipio { get; set; }
        public int cantEvaluaciones { get; set; }
        public DateTime? fechaEvaluacion { get; set; }
    }
}
