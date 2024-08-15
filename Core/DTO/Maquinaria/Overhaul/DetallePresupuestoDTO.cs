using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class DetallePresupuestoDTO
    {
        public int maquinaID { get; set; }
        public int tipo { get; set; }
        public DateTime fecha { get; set; }
        public string noEconomico { get; set; }
        public int componenteID { get; set; }
        public decimal target { get; set; }
        public string noComponente { get; set; }
        public decimal horometroCiclo { get; set; }
        public decimal horometroAcumulado { get; set; }
        public decimal costo { get; set; }
        public decimal costoSugerido { get; set; }
        public string color { get; set; }
        public int vida { get; set; }
        public bool guardado { get; set; }
        public bool esServicio { get; set; }
    }
}
