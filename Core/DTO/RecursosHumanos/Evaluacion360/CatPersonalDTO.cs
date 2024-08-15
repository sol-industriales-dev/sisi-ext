using Core.Enum.RecursosHumanos.Evaluacion360;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Evaluacion360
{
    public class CatPersonalDTO
    {
        #region SQL
        public int id { get; set; }
        public int idUsuario { get; set; }
        public int idEmpresa { get; set; }
        public int idTipoUsuario { get; set; }
        public string telefono { get; set; }
        public int nivelAcceso { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONALES
        public string cc { get; set; }
        public string descripcion { get; set; }
        public string descripcionCC { get; set; }
        public string correo { get; set; }
        public int puesto { get; set; }
        public string descripcionPuesto { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string desc_depto { get; set; }
        public string nombreCompleto { get; set; }
        public string tipoUsuario { get; set; }
        #endregion
    }
}