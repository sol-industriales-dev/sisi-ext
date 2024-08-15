using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.DTO;
using Core.DTO.Principal.Generales;
using Core.DTO;

namespace Core.DTO.Administracion.AgendarJunta
{
    public class AgendarJuntaDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string fechaInicial { get; set; }
        public string fechaFinal { get; set; }
        public string sala { get; set; }
        public string tipo { get; set; }
        public string repeticion { get; set; }
        public string tipoRep { get; set; }
        public bool estatus { get; set; }
        public int idUsu { get; set; }
        public string usuario { get; set; }
        public int numRep { get; set; }
        public string fechaRep { get; set; }





    }
}
