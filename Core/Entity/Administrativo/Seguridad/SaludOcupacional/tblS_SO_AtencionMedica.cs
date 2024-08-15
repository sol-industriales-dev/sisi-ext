using Core.Enum.Administracion.Seguridad.SaludOcupacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.SaludOcupacional
{
    public class tblS_SO_AtencionMedica
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public DateTime fecha { get; set; }
        public TipoAtencionMedicaEnum tipo { get; set; }
        public string rutaArchivoST7 { get; set; }
        public string rutaArchivoST2 { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
