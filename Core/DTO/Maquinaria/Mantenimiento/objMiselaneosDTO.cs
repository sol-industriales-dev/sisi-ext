using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Mantenimiento
{
    public class objMiselaneosDTO
    {
        public string componente { get; set; }

        public List<lubricanteCboDTO> lubricante { get; set; }
        public string cantidad { get; set; }

        public string chkProgramado { get; set; }
        public int modeloEquipoID { get; set; }

        public int idCompVis { get; set; }
        public int modelo { get; set; }

        public int idFiltro { get; set; }
        public bool aplicar { get; set; }
        public bool programado { get; set; }

        public int idMant { get; set; }
        public int tipoPMid { get; set; }
    }
}
