using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;

namespace Core.Entity.Administrativo.AgendarJunta
{
    public class tblP_SalaJunta
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public string sala { get; set; }
        public string tipo { get; set; }
        public string repeticion { get; set; }
        public string tipoRep { get; set; }
        public bool estatus { get; set; }
        public string color { get; set; }
        public int idUsu { get; set; }
        public string usuario { get; set; }
        public int numRep { get; set; }
        public string fechaRep { get; set; }

        //public int? usuarioRegistroID { get; set; }
        //public virtual tblP_Usuario usuarioRegistro { get; set; }


    }
}
