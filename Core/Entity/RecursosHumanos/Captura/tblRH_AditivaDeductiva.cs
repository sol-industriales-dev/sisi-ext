using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    //raguilar 23/11/17
    public class tblRH_AditivaDeductiva
    {
        public int id { get; set; }
        public DateTime fecha_Alta { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int usuarioCap { get; set; }
        public bool aprobado { get; set; }
        public bool rechazado { get; set; }
        public string cC { get; set; }
        public string nomUsuarioCap { get; set; }
        public string folio { get; set; }
        public string cCid { get; set; }
        public bool editable { get; set; }
        public string condicionInicial { get; set; }
        public string condicionActual { get; set; }
        public string soporte { get; set; }
        public string link { get; set; }
    }
}
