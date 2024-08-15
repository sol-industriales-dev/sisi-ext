using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class ActividadOverhaulDTO
    {
        public int id { get; set; }
        public int idAct { get; set; }
        public DateTime ? fechaInicio { get; set; }
        public int estatus { get; set; }
        public DateTime? fechaFin { get; set; }
        public decimal horasDuracion { get; set; }
        public DateTime? fechaInicioP { get; set; }
        public DateTime? fechaFinP { get; set; }
        public int numDia { get; set; }
    }
}

