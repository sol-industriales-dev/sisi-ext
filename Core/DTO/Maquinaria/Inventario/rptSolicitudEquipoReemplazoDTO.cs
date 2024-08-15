using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class rptSolicitudEquipoReemplazoDTO
    {
        public string Comentario { get; set; }
        public string Economico { get; set; }
        public string Grupo { get; set; }
        public string Modelo { get; set; }
        public string Tipo { get; set; }
        public int id { get; set; }
        public string FechaEntrega { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
    }
}
