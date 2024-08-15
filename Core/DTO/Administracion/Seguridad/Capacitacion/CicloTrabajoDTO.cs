using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class CicloTrabajoDTO
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public TipoCicloEnum tipoCiclo { get; set; }
        public string tipoCicloDesc { get; set; }
        public DateTime? fechaCiclo { get; set; }
        public string fechaCicloString { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string fechaCreacionString { get; set; }

        public string criterio { get; set; }
        public string ponderacion { get; set; }
        public PonderacionCriterioEnum ponderacionn { get; set; }

        public int area { get; set; }
        public string cc { get; set; }
        public int empresa { get; set; }
        public bool estatus { get; set; }
        public int cicloID { get; set; }
        public string departamento { get; set; }     

        public List<CicloTrabajoCriterioDTO> listaCriterios { get; set; }
    }
}
