using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Subcontratistas.Evaluacion
{
    public class DivReqDTO
    {
        public string DivicionesORequerimiento { get; set; }
        public string Pesimo { get; set; }
        public string Malo { get; set; }
        public string Regular { get; set; }
        public string Aceptable { get; set; }
        public string Excdediendo { get; set; }
        public string Calificacion { get; set; }
        public decimal CalificacionNumero { get; set; }
        public string TituloP { get; set; }
        public string Titulo { get; set; }
    }
}
