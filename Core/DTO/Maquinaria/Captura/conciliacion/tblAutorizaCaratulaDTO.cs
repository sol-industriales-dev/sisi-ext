using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.conciliacion
{
    public class tblAutorizaCaratulaDTO
    {

        public int id { get; set; }
        public int usuarioElaboraID { get; set; }
        public int usuarioVobo1 { get; set; }
        public int usuarioVobo2 { get; set; }
        public int usuarioAutoriza { get; set; }
        public string usuarioElaboraNombre { get; set; }
        public string usuarioVobo1Nombre { get; set; }
        public string usuarioVobo2Nombre { get; set; }
        public string usuarioAutorizaNombre { get; set; }
        public string cadenaElabora { get; set; }
        public string cadenaVobo1 { get; set; }
        public string cadenaVobo2 { get; set; }
        public string cadenaAutoriza { get; set; }
        public int firmaElabora { get; set; }
        public int firmaVobo1 { get; set; }
        public int firmaVobo2 { get; set; }
        public int firmaAutoriza { get; set; }
        public int estadoCaratula { get; set; }
        public int usuarioFirma { get; set; }
        public int caratulaID { get; set; }
        public int obraID { get; set; }
        public int caratulaActualID { get; set; }


    }
}
