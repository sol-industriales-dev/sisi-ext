using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Evaluacion360
{
    public class tblRH_Eval360_CatPersonal
    {
        #region SQL
        public int id { get; set; }
        public int idUsuario { get; set; }
        public int idEmpresa { get; set; }
        public int idTipoUsuario { get; set; }
        public string telefono { get; set; }
        public int nivelAcceso { get; set; }
        public bool esPrimerEnvioCorreo { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
