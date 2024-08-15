using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.CatNotificantes
{
    public class tblRH_Notis_RelConceptoUsuario
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public int idConcepto { get; set; }
        public string cc { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
