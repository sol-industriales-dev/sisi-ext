using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class LiderInfoDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public List<string> ccs { get; set; }
        public int? grupoId { get; set; }
        public List<int> lstGruposID { get; set; }
    }
}
