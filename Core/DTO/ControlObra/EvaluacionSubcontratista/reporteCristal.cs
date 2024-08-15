using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.EvaluacionSubcontratista
{
    public class reporteCristal
    {
        public int id { get; set; }
        public string Divicion { get; set; }
        public string Desviaciones { get; set; }
        public string PlanesDeAccion { get; set; }
        public string Responsable { get; set; }
        public string FechaCompromiso { get; set; }
        public int numeroMayor { get; set; }
        public decimal Calificacion { get; set; }
    }
}
