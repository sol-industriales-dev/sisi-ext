using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class SeguimientoDTO
    {
        #region ADICIONAL
        public int id { get; set; }
        public int auditoriaId { get; set; }
        public int inspeccionId { get; set; }
        public string descripcion { get; set; }
        public int respuesta { get; set; }
        public string accion { get; set; }
        public int usuario5sId { get; set; }
        public DateTime fecha { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
