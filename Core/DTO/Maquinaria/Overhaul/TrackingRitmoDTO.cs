using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class TrackingRitmoDTO
    {
        public tblM_trackComponentes tracking {get; set;}
        public decimal ritmoMaquina { get; set; }
    }
}
