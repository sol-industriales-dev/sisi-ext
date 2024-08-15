using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class StandByNuevoDTO
    {
        public int id { get; set; }
        public string Economico { get; set; }
        public int noEconomicoID { get; set; }
        public int estatus { get; set; }
        public int usuarioID { get; set; }
        public DateTime fecha { get; set; }
        public string cc { get; set; }
        public string comentario { get; set; }
        public string modelo { get; set; }
        public string ccActual { get; set; }
        public string ccDescripcion { get; set; }
        public decimal moiEquipo { get; set; }
        public decimal valorEnLibroEquipo { get; set; }
        public decimal depreciacionMensualEquipo { get; set; }
        public decimal valorEnLibroOverhaul { get; set; }
        public decimal depreciacionMensualOverhaul { get; set; }
        public string fechaCaptura { get; set; }
        public string justificacion { get; set; }
        public int usuarioCapturaID { get; set; }
    }
}
