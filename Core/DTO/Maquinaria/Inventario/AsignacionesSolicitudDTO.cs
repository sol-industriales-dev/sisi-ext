using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class AsignacionesSolicitudDTO
    {
        public string TipoEquipo { get; set; }
        public string GrupoEquipo { get; set; }
        public string ModeloEquipo { get; set; }
        public string Folio { get; set; }
        public int idSolicitud { get; set; }
    }
}
