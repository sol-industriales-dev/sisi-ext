using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class DepreciacionLugarDTO
    {
        public tblM_CatMaquina equipo { get; set; }
        public tblM_ControlEnvioMaquinaria control { get; set; }
        public tblM_STB_CapturaStandBy standBy { get; set; }
        public DateTime fechaDepreciacion { get; set; }
    }
}
