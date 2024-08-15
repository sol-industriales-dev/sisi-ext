using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_BitacoraControlAceiteMant
    {
        public int id { get; set; }
        public decimal Hrsaplico { get; set; }
        public decimal VidaRestante { get; set; }
        public decimal Vigencia { get; set; }
        public int idComp { get; set; }
        public int idMisc { get; set; }
        public bool prueba { get; set; }
        public decimal vidaActual { get; set; }
        public int idMant { get; set; }
        public bool Aplicado { get; set; }
        public int UsuarioCap { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool alta { get; set; }
        public int idAct { get; set; }
        public bool estatus { get; set; }
    }
}
