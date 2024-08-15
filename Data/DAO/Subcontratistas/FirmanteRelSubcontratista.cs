using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Subcontratistas
{
    public class FirmanteRelSubcontratista
    {
        #region TABLA SQL
        public int id { get; set; }
        public int idUsuarioSubcontratista { get; set; }
        public int nombreFirmante { get; set; }
        public string correo { get; set; }
        public bool registroHistorial { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
