using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class AsignacionDTO
    {
        public string pFechaInicio { get; set; }
        public string pFechaFin { get; set; }
        public string pHoras { get; set; }
        public string CC { get; set; }
        public string Tipo { get; set; }
        public string Modelo { get; set; }
        public string Grupo { get; set; }
        public string Descripcion { get; set; }
        public string localizacion { get; set; }
        public string Economico { get; set; }
        public string Marca { get; set; }
        public string Serie { get; set; }
        public int idEconomico { get; set; }
        public string pTipoPrioridad { get; set; }
        public int idsolicitud { get; set; }
        public string CCOrigen { get; set; }
        public string CCDestino { get; set; }
        public string folio { get; set; }
        public int estatus { get; set; }
        public int id { get; set; }
        public int estatusDetSolicitud { get; set; }
        public int SolicitudDetalleId { get; set; }

        public string FechaPromesa { get; set; }
    }
}
