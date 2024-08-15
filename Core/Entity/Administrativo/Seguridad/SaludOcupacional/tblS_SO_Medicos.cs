using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.SaludOcupacional
{
    public class tblS_SO_Medicos
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string puesto { get; set; }
        public string cedulaProfesional { get; set; }
        public string empresa { get; set; }
        public int idUsuarioSIGOPLAN { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
