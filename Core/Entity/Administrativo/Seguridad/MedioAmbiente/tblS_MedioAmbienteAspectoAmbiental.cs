using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.MedioAmbiente;

namespace Core.Entity.Administrativo.Seguridad.MedioAmbiente
{
    public class tblS_MedioAmbienteAspectoAmbiental
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool peligroso { get; set; }
        public UnidadEnum unidad { get; set; }
        public int clasificacion { get; set; }
        public bool esSolidoImpregnadoHidrocarburo { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool esActivo { get; set; }

        public virtual List<tblS_MedioAmbienteResiduoFactorPeligro> factoresPeligro { get; set; }
    }
}
