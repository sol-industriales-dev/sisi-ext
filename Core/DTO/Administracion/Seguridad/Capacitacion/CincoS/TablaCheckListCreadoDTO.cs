using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class TablaCheckListCreadoDTO
    {
        public int checkListId { get; set; }
        public List<string> ccSinCalendario { get; set; }
        public List<string> ccConCalendario { get; set; }
        public string nombreAuditoria { get; set; }
        public string area { get; set; }
        public List<string> lideres { get; set; }
    }
}
