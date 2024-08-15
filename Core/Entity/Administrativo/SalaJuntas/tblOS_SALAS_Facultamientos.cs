using Core.Enum.Administracion.AgendarJunta;
using System;

namespace Core.Entity.Administrativo.SalaJuntas
{
    public class tblOS_SALAS_Facultamientos
    {
        #region SQL
        public int id { get; set; }
        public int FK_Usuario { get; set; }
        public TipoFacultamientosEnum tipoFacultamiento { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
