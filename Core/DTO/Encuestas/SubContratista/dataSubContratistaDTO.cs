using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas.SubContratista
{
    public class dataSubContratistaDTO
    {
        public string nombreSubContratista { get; set; }
        public int numProveedor { get; set; }
        public string nombreProyecto { get; set; }
        public string centroCostos { get; set; }
        public string centroCostosNombre { get; set; }
        public string servicioContrato { get; set; }
        public int convenio { get; set; }
        public bool estatus { get; set; }
        public List<int> id { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public List<string> evaluador { get; set; }
        public List<string> fechaEvaluacion { get; set; }
        public decimal? calificacion { get; set; }

        public string Comentarios {get;set;}
        public string btn { get; set; }

        public dataSubContratistaDTO()
        {
            this.id = new List<int>();
            this.evaluador = new List<string>();
            this.fechaEvaluacion = new List<string>();
        }

    }
}
