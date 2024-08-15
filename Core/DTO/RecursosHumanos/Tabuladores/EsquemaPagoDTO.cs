using System;

namespace Core.DTO.RecursosHumanos.Tabuladores
{
    public class EsquemaPagoDTO
    {
        #region SQL
        public int id { get; set; }
        public string concepto { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}