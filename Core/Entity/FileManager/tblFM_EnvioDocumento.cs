using System;

namespace Core.Entity.FileManager
{
    public class tblFM_EnvioDocumento
    {
        public int id { get; set; }
        public int tipoDocumento { get; set; }
        public string descripcion { get; set; }
        public int documentoID { get; set; }
        public int usuarioID { get; set; }
        public int estatus { get; set; }
        public int ? empresa { get; set; }
        public DateTime fecha { get; set; }
    }
}