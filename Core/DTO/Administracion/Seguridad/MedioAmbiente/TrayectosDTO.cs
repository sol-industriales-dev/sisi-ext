using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.MedioAmbiente
{
    public class TrayectosDTO
    {
        public int id { get; set; }
        public int idAgrupacion { get; set; }
        public int idAspectoAmbiental { get; set; }
        public string tratamiento { get; set; }
        public string manifiesto { get; set; }
        public DateTime fechaEmbarque { get; set; }
        public string tipoTransporte { get; set; }
        public int idTransporteTrayecto { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
