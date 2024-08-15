using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class AsignadosDTO
    {
        public int id { get; set; }
        public string Folio { get; set; }
        public int noEconomicoID { get; set; }
        public string Economico { get; set; }
        public string TipoEquipo { get; set; }
        public string pFechaInicio { get; set; }
        public string pFechaFin { get; set; }
        public string FechaPromesa { get; set; }
        public string Comentario { get; set; }
        public string Tipo { get; set; }
        public string Grupo { get; set; }
        public string Modelo { get; set; }
        public int tipoAsignacion { get; set; }
    }
}
