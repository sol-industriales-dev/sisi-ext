using Core.Enum.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class CicloTrabajoCriterioDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public PonderacionCriterioEnum ponderacion { get; set; }
        public AspectoEvaluadoCriterioEnum aspectoEvaluado { get; set; }
        public string ponderacionDesc { get; set; }
        public int cicloTrabajoID { get; set; }
    }
}
