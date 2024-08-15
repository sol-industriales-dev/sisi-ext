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
    public class PresupuestoOverhaulDTO
    {
        public int subconjuntoID { get; set; }
        public string subconjunto { get; set; }
        public List<string> maquinasComponentes { get; set; }
        public List<PropiedadOverhaulDTO> obras { get; set; }
        public int eventoID { get; set; }
        public List<tblM_DetallePresupuestoOverhaul> detalles { get; set; }
        public decimal costoTotal { get; set; }

        public bool esServicio { get; set; }

        public List<decimal> costoVida { get; set; }
        public List<int> vida { get; set; }
        public int totalComponentes { get; set; }
    }
}