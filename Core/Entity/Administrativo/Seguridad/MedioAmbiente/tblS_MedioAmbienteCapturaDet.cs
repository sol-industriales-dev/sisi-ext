using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.MedioAmbiente
{
    public class tblS_MedioAmbienteCapturaDet
    {
        public int id { get; set; }
        public int idCaptura { get; set; }
        public string codigoContenedor { get; set; }
        public int consecutivoCodContenedor { get; set; }
        public int idAspectoAmbiental { get; set; }
        public decimal cantAspectoAmbiental { get; set; }
        public int estatusAspectoAmbiental { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
