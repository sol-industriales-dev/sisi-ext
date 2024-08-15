using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class TrackingHistorialDTO
    {
        public int componenteID { get; set; }
        public DateTime fecha { get; set; }
        public string locacion { get; set; }
        public bool reciclado { get; set; }
        public bool tipoLocacion { get; set; }
        public int id { get; set; }
        public decimal horasHistorial { get; set; }
    }
}
