using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.BackLogs
{
    public class tblBL_Partes
    {
        #region SQL
        public int id { get; set; }
        public int idBacklog { get; set; }
        public int insumo { get; set; }
        public int cantidad { get; set; }
        public string parte { get; set; }
        public string articulo { get; set; }
        public string unidad { get; set; }
        public int tipoMoneda { get; set; }
        public decimal costoPromedio { get; set; }
        public string PERU_insumo { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool esActivo { get; set; }
        #endregion
    }
}