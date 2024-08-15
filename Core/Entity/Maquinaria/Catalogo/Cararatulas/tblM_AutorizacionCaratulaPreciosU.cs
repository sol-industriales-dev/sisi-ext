using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Catalogo.Cararatulas
{
    public class tblM_AutorizacionCaratulaPreciosU
    {
        public int id { get; set; }
        public int usuarioElaboraID { get; set; }
        public int usuarioVobo1 { get; set; }
        public int usuarioVobo2 { get; set; }
        public int usuarioAutoriza { get; set; }
        public string cadenaElabora { get; set; }
        public string cadenaVobo1 { get; set; }
        public string cadenaVobo2 { get; set; }
        public string cadenaAutoriza { get; set; }
        public int firmaElabora { get; set; }
        public int firmaVobo1 { get; set; }
        public int firmaVobo2 { get; set; }
        public int firmaAutoriza { get; set; }
        public DateTime fechaElaboracion { get; set; }
        public DateTime? fechaVobo1 { get; set; }
        public DateTime? fechaAutoriza { get; set; }
        public DateTime? fechaVobo2 { get; set; }

        public int estadoCaratula { get; set; }
        public int usuarioFirma { get; set; }
        public int caratulaID { get; set; }
        public int obraID { get; set; }
        public string comentario { get; set; }

    }

}
