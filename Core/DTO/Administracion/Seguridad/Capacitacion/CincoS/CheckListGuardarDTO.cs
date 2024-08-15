using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class CheckListGuardarDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public List<string> ccs { get; set; }
        public int areaId { get; set; }
        public List<int> lideresId { get; set; }
        public List<InspeccionGuardarDTO> inspecciones { get; set; }

        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public int usuarioModificacionId { get; set; }
    }
}
