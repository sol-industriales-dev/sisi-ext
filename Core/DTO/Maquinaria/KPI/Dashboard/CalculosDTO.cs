using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.KPI.Dashboard
{
    public class CalculosDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public TablaTiemposDTO tiempos { get; set; }

        public int idGrupo { get; set; }
        public string grupoDescripcion { get; set; }
        public int idModelo { get; set; }
        public string modeloDescripcion { get; set; }

        public CalculosDTO()
        {
            tiempos = new TablaTiemposDTO();
        }
    }
}
