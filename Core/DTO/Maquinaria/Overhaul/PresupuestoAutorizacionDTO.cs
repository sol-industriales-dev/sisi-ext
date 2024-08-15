using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Overhaul;
using Core.Entity.Principal.Multiempresa;
using Core.DTO.Maquinaria.Overhaul;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class PresupuestoAutorizacionDTO
    {
        public decimal presupuesto { get; set; }
        public int numComponentes { get; set; }
        public decimal avancePresupuesto { get; set; }
        public int numPresupuestados { get; set; }
    }
}
