using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class SolicitudesDetalleVacantesDTO
    {
        public string CentroCostos { get; set; }
        public int noSolicitudes { get; set; }
        public int noSolicitudesAceptadas { get; set; }
        public int noSolicitudesPendientes { get; set; }
        public int TotalDeVacantes { get; set; }
        public int TotalOcupadas { get; set; }
        public int TotalLibres { get; set; }
        public string Folio { get; set; }

    }
}
