using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class SeguimientoAccionDTO
    {
        public int id { get; set; }
        public string accion { get; set; }
        public int metodo { get; set; }
        public string metodoDesc { get; set; }
        public string rutaEvidencia { get; set; }
        public int evaluador { get; set; }
        public string evaluadorDesc { get; set; }
        public bool aprobo { get; set; }
        public string comentariosEvaluador { get; set; }
        public int colaborador { get; set; }
        public string colaboradorDesc { get; set; }
        public DateTime? fecha { get; set; }
        public string fechaString { get; set; }
        public string areaSeguimientoString { get; set; }
        public string interesadosString { get; set; }
    }
}
