using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.ControlObra;

namespace Core.DTO.ControlObra
{
    public class FirmaDTO
    {
        public int firma_id { get; set; }
        public int firmante_id { get; set; }
        public TipoFirmanteEnum tipo { get; set; }
        public string tipoDesc { get; set; }
        public string nombre { get; set; }
        public DateTime? fechaAutorizacion { get; set; }
        public string fechaAutorizacionString { get; set; }
        public EstadoFirmaEnum estadoFirma { get; set; }
        public string estadoFirmaDesc { get; set; }
        public bool flagPuedeFirmar { get; set; }
        public bool flagPuedeRechazar { get; set; }
    }
}
