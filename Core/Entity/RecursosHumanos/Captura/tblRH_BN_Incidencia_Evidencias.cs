using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_Incidencia_Evidencias
    {
        #region SQL
        public int id { get; set; }
        public int incidenciaID { get; set; }
        public int incidencia_detID { get; set; }
        public int clave_empleado { get; set; }
        public string ruta { get; set; }
        public string nombreArchivo { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
