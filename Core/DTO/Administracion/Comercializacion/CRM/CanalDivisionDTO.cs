using System;

namespace Core.DTO.Administracion.Comercializacion.CRM
{
    public class CanalDivisionDTO
    {
        #region SQL
        public int id { get; set; }
        public int FK_Canal { get; set; }
        public int FK_Division { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string canal { get; set; }
        public string division { get; set; }
        #endregion
    }
}