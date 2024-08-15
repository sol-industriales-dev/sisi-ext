using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Encuestas
{
    public class EncuestaResults2DTO
    {
        public int preguntaID { get; set; }
        public string pregunta { get; set; }
        public decimal enero{ get; set; }
        public decimal febrero { get; set; }
        public decimal marzo { get; set; }
        public decimal abril { get; set; }
        public decimal mayo { get; set; }
        public decimal junio { get; set; }
        public decimal julio { get; set; }
        public decimal agosto { get; set; }
        public decimal septiembre { get; set; }
        public decimal octubre { get; set; }
        public decimal noviembre { get; set; }
        public decimal diciembre{ get; set; }
        public decimal total { get; set; }
        public int tipo { get; set; }
        public int muestra { get; set; }
        public int tresestrellasCont { get; set; }
        public string tresestrellasPerc { get; set; }
        public decimal trim1 { get; set; }
        public decimal trim2 { get; set; }
        public decimal trim3 { get; set; }
        public decimal trim4 { get; set; }
        public string encuesta { get; set; }
    }
}
