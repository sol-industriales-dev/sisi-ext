using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_PP_PlantillaPersonalDTO
    {
        public string id { get; set; }
        public string ccID { get; set; }
        public string cc { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public string usuarioID { get; set; }
        public string fechaMod { get; set; }
        public string estatus { get; set; }
        public List<tblRH_PP_PlantillaPersonal_DetDTO> Detalle { get; set; }
    }
}
