using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Utils.Data
{
    public class StoreProcedureDTO
    {
        /// <summary>
        /// Asigna el contexto de base de datos. Si vale 0 selecciona la empresa en turno.
        /// </summary>
        public MainContextEnum baseDatos { get; set; }
        /// <summary>
        /// Nombre del procedimiento
        /// </summary>
        public string nombre { get; set; }
        public List<OdbcParameterDTO> parametros { get; set; }
        public StoreProcedureDTO()
        {
            parametros = new List<OdbcParameterDTO>();
        }
    }
}
