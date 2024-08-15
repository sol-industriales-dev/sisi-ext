using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.MedioAmbiente
{
    public class TransportistasDTO
    {
        public int id { get; set; }
        public string razonSocial { get; set; }
        public string numAutorizacion { get; set; }
        public int idClasificacion { get; set; }
        public string clasificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
