using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte
{
    public class area_cuentaDTO
    {
        //area, cuenta,descripcion

        public int area { get; set; }
        public int cuenta { get; set; }
        public string descripcion { get; set; }
        public string centro_costos { get; set; }

    }
}
