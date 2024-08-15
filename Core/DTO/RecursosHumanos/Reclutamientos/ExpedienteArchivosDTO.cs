using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class ExpedienteArchivosDTO
    {
        #region TABLA SQL
        public int id { get; set; }
        public string descripcion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
