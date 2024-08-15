using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Subcontratistas;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enum.ControlObra;
using Core.Entity.ControlObra;

namespace Core.Entity.SubContratistas
{
    public class tblX_Contrato
    {
        public int id { get; set; }
        public string numeroContrato { get; set; }
        public string cc { get; set; }
        public DateTime? fechaSuscripcion { get; set; }
        public DateTime? fechaVigencia { get; set; }
        public decimal montoContractual { get; set; }
        public bool anticipoAplica { get; set; }
        public decimal anticipoPorcentaje { get; set; }
        public string penalizacion { get; set; }
        public AreasEnum area { get; set; }
        public int proyectoID { get; set; }
        public int subcontratistaID { get; set; }
        public int estatusContratoId { get; set; }
        public DateTime? fechaTerminacion { get; set; }
        public DateTime? fechaInicial { get; set; }
        public DateTime? fechaFinal { get; set; }
        public int estado_id { get; set; }
        public int municipio_id { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("estatusContratoId")]
        public virtual tblX_EstatusContrato estatusContrato { get; set; }

        [ForeignKey("contratoID")]
        public virtual List<tblX_ContratoPeriodo> periodosCapturados { get; set; }

        [ForeignKey("subcontratistaID")]
        public virtual tblX_SubContratista subcontratista { get; set; }

        [ForeignKey("proyectoID")]
        public virtual tblX_Proyecto proyecto { get; set; }

        [ForeignKey("idContrato")]
        public virtual List<tblCO_ADP_EvalSubConAsignacion> evaluaciones { get; set; }
    }
}
