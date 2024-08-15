using Core.Entity.Principal.Usuarios;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.FileManager
{
    public class tblFM_Version
    {
        #region SQL
        public long id { get; set; }
        public long archivoID { get; set; }
        public int usuarioCreadorID { get; set; }
        public int version { get; set; }
        public string ruta { get; set; }
        public string nombre { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaEdicion { get; set; }
        public int numeroArchivo { get; set; }
        public bool activo { get; set; }
        public string abreviacion { get; set; }
        public bool considerarse { get; set; }
        public bool obraCerrada { get; set; }
        #endregion

        #region ADICIONAL
        public virtual tblFM_Archivo archivo { get; set; }
        public virtual tblP_Usuario usuario { get; set; }

        public override string ToString()
        {
            return String.Format("{0}[{1}] - {2}", nombre, abreviacion, ruta);
        }
        #endregion
    }
}
