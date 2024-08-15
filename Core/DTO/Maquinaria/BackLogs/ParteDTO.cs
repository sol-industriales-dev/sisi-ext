using System;
using System.Collections.Generic;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class ParteDTO
    {
        #region INIT
        public ParteDTO()
        {
            lstBackLogsID = new List<int>();
        }
        #endregion

        #region SQL
        public int id { get; set; }
        public int idBacklog { get; set; }
        public int insumo { get; set; }
        public int cantidad { get; set; }
        public string parte { get; set; }
        public string articulo { get; set; }
        public string unidad { get; set; }
        public int tipoMoneda { get; set; }
        public string costoPromedio { get; set; }
        public string PERU_insumo { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool esActivo { get; set; }
        #endregion

        #region ADICIONAL
        public string strMoneda { get; set; }
        public List<int> lstBackLogsID { get; set; }
        #endregion
    }
}