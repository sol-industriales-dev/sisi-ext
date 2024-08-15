using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.CatNotificantes
{
    public class tblRH_Notis_RelConceptoCorreo
    {
        public int id { get; set; }
        public string correo { get; set; }
        public int idConcepto { get; set; }
        public string cc { get; set; }
        public System.DateTime fechaCreacion { get; set; }
        public System.DateTime fechaModificacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public bool esActivo { get; set; }
    }
}
