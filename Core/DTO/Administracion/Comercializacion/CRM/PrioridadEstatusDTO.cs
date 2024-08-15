using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Comercializacion.CRM
{
    public class PrioridadEstatusDTO
    {
        #region SQL
        public int id { get; set; }
        public int FK_Prioridad { get; set; }
        public string estatus { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
