using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.OT.rptConcentradoHH
{
    public class GraficaParetoDTO
    {
        public List<ParetoCategoriasDTO> listaCategorias { get; set; }
        public List<ParetoCategoriasDTO> listaSubCategorias { get; set; }
    }
}
