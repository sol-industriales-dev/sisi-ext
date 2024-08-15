using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_AutorizacionStandBy
    {
        public int id { get; set; }
        public int usuarioSolicita { get; set; }
        public int usuarioAutoriza { get; set; }
        public string comentario { get; set; }
        public DateTime fechaSolicitud { get; set; }
        public bool estatus { get; set; }
        public int autorizacion { get; set; }
        public int tipoStandBy { get; set; }

        public int idEconomico { get; set; }
        public decimal horasParo { get; set; }
        public int idAsignacion { get; set; }

        public string comentarioSolicitud { get; set; }
        public DateTime fechaAutorizacion { get; set; }

        public string CC { get; set; }
    }

}
