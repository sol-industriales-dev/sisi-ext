using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.EvaluacionSubcontratista
{
    public class RequerimientosDTO
    {
        public int id { get; set; }
        public int idDiv { get; set; }
        public string texto { get; set; }
        public string inputFile { get; set; }
        public string lblInput { get; set; }
        public string tipoFile { get; set; }
        public string txtAComentario { get; set; }
        public string txtPlaneacion { get; set; }
        public string txtResponsable { get; set; }
        public string txtFechaCompromiso { get; set; }
        public bool important { get; set; }
        public bool evaluado { get; set; }
        public bool tieneRetroalimentacion { get; set; }

    }
}
