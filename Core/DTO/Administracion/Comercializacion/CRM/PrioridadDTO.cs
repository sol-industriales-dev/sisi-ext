using System;
using System.Collections.Generic;

namespace Core.DTO.Administracion.Comercializacion.CRM
{
    public class PrioridadDTO
    {
        #region INIT
        public PrioridadDTO()
        {
            lstID_Prioridades = new List<int>();
        }
        #endregion

        #region SQL
        public int id { get; set; }
        public string prioridad { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public List<int> lstID_Prioridades { get; set; }
        #endregion
    }
}