using Core.Entity.Encuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas.Proveedores
{
    public class ObjGraficasDTO
    {
        public List<tblEN_ResultadoProveedores> ProveedoresList { get; set; }
        public List<EvaluacionCompradoresDTO> CompradoresList { get; set; }
        public List<tblEN_ResultadoProveedorRequisiciones> ProveedoresListOC { get; set; }

        public List<string> Preguntas { get; set; }
        public List<decimal> Calificaciones { get; set; }

        public List<string> ProveedoresMasEvaluados { get; set; }
        public List<decimal> CalificacionesMasEvaluados { get; set; }

        public List<string> ProveedoresPeorEvaluados { get; set; }
        public List<decimal> CalificacionesPeorEvaluados { get; set; }

        public List<string> ProveedoresBest { get; set; }
        public List<decimal> CalificacionesBest { get; set; }

        public ObjGraficasDTO()
        {
            this.Preguntas = new List<string>();
            this.Calificaciones = new List<decimal>();

            this.ProveedoresMasEvaluados = new List<string>();
            this.CalificacionesMasEvaluados = new List<decimal>();

            this.ProveedoresPeorEvaluados = new List<string>();
            this.CalificacionesPeorEvaluados = new List<decimal>();

            this.ProveedoresBest = new List<string>();
            this.CalificacionesBest = new List<decimal>();
        }
    }
}
