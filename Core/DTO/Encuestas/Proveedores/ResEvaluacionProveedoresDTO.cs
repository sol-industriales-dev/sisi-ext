using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas.Proveedores
{
    public class ResEvaluacionProveedoresDTO
    {
        public int proveedorID { get; set; }
        public string proveedorName { get; set; }
        public int cantidadPesimos { get; set; }
        public int cantidadMalos { get; set; }
        public int cantidadRegulares { get; set; }
        public int cantidadAceptables { get; set; }
        public int cantidadEstupendos { get; set; }
        public int cantidadBuenos { get; set; }
        public string tipoProveedor { get; set; }
        public string comentario { get; set; }
        public int folioID { get; set; }
        public decimal porcentaje { get; set; }

        public string tipoMoneda { get; set; }

        public string nombreEvaluador { get; set; }
        public DateTime fechaEvaluacion { get; set; }
    }
}