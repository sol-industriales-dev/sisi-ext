using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.BajasPersonal
{
    public class HistorialCCDTO
    {
        public string cc_origen { get; set; }
        public string nombreCC_origen { get; set; }
        public string puesto_origen { get; set; }
        public string jefe_origen { get; set; }
        public string patron_origen { get; set; }

        public string cc_nuevo { get; set; }
        public string nombreCC_nuevo { get; set; }
        public string puesto_nuevo { get; set; }
        public string jefe_nuevo { get; set; }
        public string patron_nuevo { get; set; }


        public string fechaInicio { get; set; }
       

    }
}
