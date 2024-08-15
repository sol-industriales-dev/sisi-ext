using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class EventoPlaneacionOverhaulDTO
    {
        public decimal cicloVidaHoras { get; set; }
        public decimal horaCicloActual  { get; set; }
        public int id  { get; set; }
        public string nombre  { get; set; }
        public string descripcion  { get; set; }
        public int tipo { get; set; }
        public int subConjuntoID { get; set; }
        public string subConjunto { get; set; }
        public int posicion { get; set; }
        public string fechaRemocion { get; set; }
    }
}