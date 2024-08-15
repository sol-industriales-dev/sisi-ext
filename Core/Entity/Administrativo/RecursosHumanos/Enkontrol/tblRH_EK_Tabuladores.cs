using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Tabuladores
    {
        public int id { get; set; }
        public string cc { get; set; }
        public DateTime fecha { get; set; }
        public string observaciones { get; set; }
        public int nomina { get; set; }
        public string libre { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int? idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool? esActivo { get; set; }
    }
}
