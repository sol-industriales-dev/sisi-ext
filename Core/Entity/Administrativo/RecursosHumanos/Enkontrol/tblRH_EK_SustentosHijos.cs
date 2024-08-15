using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_SustentosHijos
    {
        #region SQL
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public int FK_EmplFamilia { get; set; }
        public string nombreArchivo { get; set; }
        public string rutaArchivo { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
