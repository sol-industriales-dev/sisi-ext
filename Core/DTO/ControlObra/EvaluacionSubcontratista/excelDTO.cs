using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.EvaluacionSubcontratista
{
    public class excelDTO
    {
        public int id { get; set; }
        public string Divicion { get; set; }
        public List<resultDTO> Desviaciones { get; set; }
        public List<resultDTO> PlanesDeAccion { get; set; }
        public List<resultDTO> Responsable { get; set; }
        public List<resultDTO> FechaCompromiso { get; set; }
        public int numeroMayor { get; set; }
        public decimal Calificacion { get; set; }
    }
}
