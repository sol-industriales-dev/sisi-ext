using Core.Enum.RecursosHumanos.Vacaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Vacaciones
{
    public class tblRH_Vacaciones_Retardos_Gestion
    {
        public int id { get; set; }
        public int idRetardo { get; set; }
        public int idUsuario { get; set; }
        public OrdenGestionEnum orden { get; set; }
        public GestionEstatusEnum estatus { get; set; }
        public string firmaElect { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
