using System;
using System.Collections.Generic;

namespace Core.DTO.Administracion.Comercializacion.CRM
{
    public class UsuarioCRMDTO
    {
        #region INIT
        public UsuarioCRMDTO()
        {
            lstFK_Menu = new List<int>();
        }
        #endregion

        #region SQL
        public int id { get; set; }
        public int FK_Usuario { get; set; }
        public int FK_Menu { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string nombreUsuario { get; set; }
        public string menu { get; set; }
        public List<int> lstFK_Menu { get; set; }
        public string htmlMenus { get; set; }
        #endregion
    }
}