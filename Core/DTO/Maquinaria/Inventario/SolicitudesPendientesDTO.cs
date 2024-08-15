using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class SolicitudesPendientesDTO
    {

        public string Folio { get; set; }
        public string CCName { get; set; }
        public string UsuarioSolicitud { get; set; }
        public int hasAsignacion { get; set; }
        public string Fecha { get; set; }
        public int id { get; set; }
        public string cc { get; set; }

        public string CentroCostos { get; set; }
        public int tipoAsignacion { get; set; }
        public string Comentario { get; set; }
    }
}
