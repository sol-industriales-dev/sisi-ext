using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Subcontratistas.Evaluacion
{
    public class UsuarioRelSubcontratistaDTO
    {
        public int id { get; set; }
        public List<string> lstContratos { get; set; }
        public string subcontratista { get; set; }
        public int idUsuarioSubcontratista { get; set; }
        public string nombreFirmante { get; set; }
        public string correo { get; set; }
        public bool registroHistorial { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public int idEvaluacion { get; set; }
        public string numeroContrato { get; set; }
        public int idSubcontratista { get; set; }
        public string idContrato { get; set; }
        public int idContrato_ID { get; set; }
    }
}
