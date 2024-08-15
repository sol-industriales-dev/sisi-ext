using System;

namespace Core.DTO.Principal.Generales
{
    public class PaisDTO
    {
        #region SQL
        public int idPais { get; set; }
        public string Pais { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}