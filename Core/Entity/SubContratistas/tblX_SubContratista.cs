using Core.Entity.ControlObra;
using Core.Entity.SubContratistas.Maquinaria.KB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_SubContratista
    {
        public int id { get; set; }
        public int numeroProveedor { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string nombreCorto { get; set; }
        public string codigoPostal { get; set; }
        public string rfc { get; set; }
        public string correo { get; set; }
        public string rutaArchivoFirma { get; set; }
        public string representanteLegal { get; set; }
        public int? divisionId { get; set; }
        public bool fisica { get; set; }
        public bool pendienteValidacion { get; set; }
        public int tipoBloqueoId { get; set; }
        public DateTime? fechaBloqueo { get; set; }
        public bool desbloqueo { get; set; }
        public DateTime? fechaInicialDesbloqueo { get; set; }
        public DateTime? fechaFinalDesbloqueo { get; set; }
        public bool estatus { get; set; }   

        [ForeignKey("divisionId")]
        public virtual tblM_KBDivisions division { get; set; }

        [ForeignKey("tipoBloqueoId")]
        public virtual tblX_TipoBloqueo tipoBloqueo { get; set; }

        [ForeignKey("subcontratistaID")]
        public virtual List<tblX_Contrato> contratos { get; set; }

        [ForeignKey("idSubContratista")]
        public virtual List<tblCO_ADP_EvalSubConAsignacion> evaluaciones { get; set; }
    }
}
