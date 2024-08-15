using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.CatNotificantes
{
    public class RowsNotificantesDTO
    {
        public string ccDesc { get; set; }
        public string cc { get; set; }
        public string idConcepto { get; set; }
        public List<string> nombresTaller { get; set; }
        public List<string> nombresAlmacen { get; set; }
        public List<string> nombresConta { get; set; }
        public List<string> nombresNominas { get; set; }
        public List<string> nombresResponsableCC { get; set; }
        public List<string> nombresAltas { get; set; }
        public List<string> nombresBajas { get; set; }
        public List<string> nombresCH { get; set; }
        public List<string> nombresIncapacidades { get; set; }
    }
}
