using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class DetallePresupuestoOverhaul
    {
        public int id { get; set; }
        public string componenteID { get; set; }
        public string maquinaID { get; set; }
        public int eventoID { get; set; }
        public decimal costoSugerido { get; set; }
        public decimal costoPresupuesto { get; set; }
        public decimal horasCiclo { get; set; }
        public decimal horasAcumuladas { get; set; }
        public int presupuestoID { get; set; }
        public int estado { get; set; }
        public string noEconomico { get; set; }
        public int subconjuntoID { get; set; }
        public string obra { get; set; }
        public int vida { get; set; }
        public decimal costoReal { get; set; }
        public DateTime fecha { get; set; }
        public int tipo { get; set; }
        public string comentarioAumento { get; set; }
        public bool programado { get; set; }
        public bool esServicio { get; set; }


    }
}
