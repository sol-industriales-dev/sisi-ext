using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.MedioAmbiente;
using Core.Entity.Administrativo.Seguridad.MedioAmbiente;

namespace Core.DTO.Administracion.Seguridad.MedioAmbiente
{
    public class AspectoAmbientalDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool peligroso { get; set; }
        public string tipoDesc { get; set; }
        public UnidadEnum unidad { get; set; }
        public string unidadDesc { get; set; }
        public int clasificacion { get; set; }
        public string clasificacionDesc { get; set; }
        public bool esSolidoImpregnadoHidrocarburo { get; set; }

        public List<ResiduoFactorPeligroDTO> factoresPeligro { get; set; }
    }
}
