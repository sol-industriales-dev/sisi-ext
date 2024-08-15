using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class CalendarioCheckListDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public List<ComboDTO> ccs { get; set; }
        public List<ComboDTO> fechas { get; set; }
        public int checkListId { get; set; }
        public List<DateTime> fechasDateTime { get; set; }
        public List<string> fechasString { get; set; }

        public CalendarioCheckListDTO()
        {
            fechasDateTime = new List<DateTime>();
            fechasString = new List<string>();
        }
    }
}
