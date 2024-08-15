using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Tabuladores
{
    public class LineaNegocioDetDTO
    {
        #region SQL
        public int id { get; set; }
        public int FK_LineaNegocio { get; set; }
        public string cc { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public List<string> lstCC { get; set; }
        public string descripcion { get; set; }
        public string ccDescripcion { get; set; }
        public bool registroActivoCC { get; set; }
        #endregion
    }
}
