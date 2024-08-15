using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Multiempresa;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class ServiciosOverhaulActivosDTO
    {
        public int id { get; set; }
        public string centroCostos { get; set; }
        public tblM_CatMaquina maquina { get; set; }
        public string nombreServicio { get; set; }
        public List<Tuple<tblM_CatTipoServicioOverhaul, tblM_CatServicioOverhaul>> servicios { get; set; }
        public DateTime fecha { get; set; }
        public decimal vidaRestanteMaxima { get; set; }
    }
}

