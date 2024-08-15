using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
namespace Core.Entity.SeguimientoAcuerdos
{
    public class tblSA_Minuta
    {
        public int id { get; set; }
        public string proyecto { get; set; }
        public string titulo { get; set; }
        public string lugar { get; set; }
        public DateTime fecha { get; set; }
        public string horaInicio { get; set; }
        public string horaFin { get; set; }
        public string descripcion { get; set; }
        public int creadorID { get; set; }
        public virtual tblP_Usuario creador { get; set; }
        public virtual List<tblSA_Actividades> actividades { get; set; }
        public virtual List<tblSA_Participante> participantes { get; set; }
        public virtual List<tblSA_Interesados> interesados { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaCompromiso { get; set; }
    }
}
