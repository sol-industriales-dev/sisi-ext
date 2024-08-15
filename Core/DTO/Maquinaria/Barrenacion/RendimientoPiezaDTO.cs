using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Barrenacion
{
    public class RendimientoPiezaDTO
    {
        public int piezaID { get; set; }
        public string tipoPieza { get; set; }
        public string noSerie { get; set; }
        public string barrenadora { get; set; }
        public decimal horasTrabajadas { get; set; }
        public int totalBarrenos { get; set; }
        public decimal metrosLineales { get; set; }
        public decimal toneladasBarreno { get; set; }
        public decimal toneladasBarrenoRealizados { get; set; }
        public decimal metrosCubicos { get; set; }
    }
}
