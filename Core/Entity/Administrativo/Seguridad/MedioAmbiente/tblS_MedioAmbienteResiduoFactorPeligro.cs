using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.MedioAmbiente;

namespace Core.Entity.Administrativo.Seguridad.MedioAmbiente
{
    public class tblS_MedioAmbienteResiduoFactorPeligro
    {
        public int id { get; set; }
        public FactorPeligroEnum factorPeligro { get; set; }
        public int residuoID { get; set; }
        public virtual tblS_MedioAmbienteAspectoAmbiental residuo { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
