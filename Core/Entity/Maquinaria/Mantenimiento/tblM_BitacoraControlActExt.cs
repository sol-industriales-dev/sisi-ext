using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento
{
    public class tblM_BitacoraControlActExt
    {
        public int id { get; set; }
        public bool Aplicado { get; set; }
        public decimal Hrsaplico { get; set; }
        public int idPerioricidad { get; set; }
        public int idAct { get; set; }
        public decimal vidaActual { get; set; }
        public decimal vidaRestante { get; set; }
        public int idMant { get; set; }
        public int UsuarioCap { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool alta { get; set; }
    }
}
