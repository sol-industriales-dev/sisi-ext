using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class DetalleVacantesSolicitudDTO
    {
        public int Solicitud { get; set; }
        public string Grupo { get; set; }
        public string Tipo { get; set; }
        public string Folio { get; set; }
        public string Modelo { get; set; }
        public int CantidadSolicitudes { get; set; }
        public string nameCentroCostos { get; set; }
        public int CantidadVacantes { get; set; }

        public int IDGrupo { get; set; }
    }
}
