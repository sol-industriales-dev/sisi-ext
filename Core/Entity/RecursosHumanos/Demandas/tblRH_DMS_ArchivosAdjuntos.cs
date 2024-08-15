using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Demandas
{
    public class tblRH_DMS_ArchivosAdjuntos
    {
        #region SQL
        public int id { get; set; }
        public int FK_Captura { get; set; }
        public string archivo { get; set; }
        public string rutaArchivo { get; set; }
        public int tipoArchivo { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
