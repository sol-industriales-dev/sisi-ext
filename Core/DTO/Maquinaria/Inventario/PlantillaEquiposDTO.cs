using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class PlantillaEquiposDTO
    {
        public string folio { get; set; }
        public int solicitudID { get; set; }
        public string GrupoDescripcion { get; set; }
        public string TipoDescripcion { get; set; }
        public int cantidad { get; set; }
    }
}
