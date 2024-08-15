using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Constancias
{
    public class AutorizarRechazarPrestamoDTO
    {
        #region SQL
        public int id { get; set; }
        public int FK_Prestamo { get; set; }
        public string tipoAutorizante { get; set; }
        public int cveEmpleado { get; set; }
        public bool esPrestamoAutorizado { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        
        #endregion
    }
}
