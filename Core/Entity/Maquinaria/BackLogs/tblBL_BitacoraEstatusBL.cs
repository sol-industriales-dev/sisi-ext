using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.BackLogs
{
    public class tblBL_BitacoraEstatusBL
    {
        public int id { get; set; }
        public int idBL { get; set; }
        public string areaCuenta { get; set; }
        public int diasTranscurridos { get; set; }
        public int idEstatus { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
