using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.ControlObra;

namespace Core.Entity.ControlObra
{
    public class tblCOES_Firma
    {
        public int id { get; set; }
        public int evaluacion_id { get; set; }
        public int contrato_id { get; set; }
        public int firmante_id { get; set; }
        public TipoFirmanteEnum tipo { get; set; }
        public EstadoFirmaEnum estadoFirma { get; set; }
        public string rutaArchivoFirma { get; set; }
        public DateTime? fechaAutorizacion { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
