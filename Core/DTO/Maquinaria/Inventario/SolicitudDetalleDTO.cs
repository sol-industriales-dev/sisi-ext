using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class SolicitudDetalleDTO
    {
        public int id { get; set; }
        public int idPrincipal { get; set; }
        public int idTempSolicitudes { get; set; }
        public int idTempPrograma { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TipoPrioriad { get; set; }
        public int idTipoPrioridad { get; set; }
        public int HasPrograma { get; set; }
        public string Comentario { get; set; }
        public int TipoId { get; set; }
        public int GrupoId { get; set; }
        public int Modeloid { get; set; }
        public string Tipo { get; set; }
        public string Grupo { get; set; }
        public string Modelo { get; set; }
        public decimal pHoras { get; set; }
        public string pFechaInicio { get; set; }
        public string pFechaFin { get; set; }
        public string pTipoPrioridad { get; set; }
        public decimal HorasTotales { get; set; }
        public int tipoUtilizacion { get; set; }

        public string Economico { get; set; }

        public int idNoEconomico { get; set; }

    }
}
