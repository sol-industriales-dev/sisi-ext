using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class HorasHombreDTO
    {
        public string año { get; set; }
        public List<string> cc { get; set; }
        public List<CapacitacionCCDTO> comboCC { get; set; }

        public string centrocosto { get; set; }
        public string descripcion { get; set; }
        public string totalPersonal { get; set; }
        public HorasHombreAnualDTO lstGlobal { get; set; }
        public HorasHombreAnualDTO lstEnero { get; set; }
        public HorasHombreAnualDTO lstFebrero { get; set; }
        public HorasHombreAnualDTO lstMarzo { get; set; }
        public HorasHombreAnualDTO lstAbril { get; set; }
        public HorasHombreAnualDTO lstMayo { get; set; }
        public HorasHombreAnualDTO lstJunio { get; set; }
        public HorasHombreAnualDTO lstJulio { get; set; }
        public HorasHombreAnualDTO lstAgosto { get; set; }
        public HorasHombreAnualDTO lstSeptiembre { get; set; }
        public HorasHombreAnualDTO lstOctubre { get; set; }
        public HorasHombreAnualDTO lstNoviembre { get; set; }
        public HorasHombreAnualDTO lstDiciembre { get; set; }


        public string promedioHRSCapacitaciones { get; set; }
        public string promedioHRSTrabajadas { get; set; }
    }
}
