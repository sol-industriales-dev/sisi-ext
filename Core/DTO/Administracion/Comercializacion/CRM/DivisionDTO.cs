using System;
using System.Collections.Generic;

namespace Core.DTO.Administracion.Comercializacion.CRM
{
    public class DivisionDTO
    {
        #region INIT
        public DivisionDTO()
        {
            lstID_Divisiones = new List<int>();
        }
        #endregion

        #region SQL
        public int id { get; set; }
        public decimal numDivision { get; set; }
        public string division { get; set; }
        public int FK_UsuarioResponsable { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONALES
        public List<int> lstID_Divisiones { get; set; }
        #endregion
    }
}