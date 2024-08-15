using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Rastreo
{
    public class EquiposPendientesReemplazoDTO
    {

        public string ProyectoID { get; set; }
        public string Proyecto { get; set; }
        public string GrupoEquipo { get; set; }
        public string Modelo { get; set; }
        public string Prioridad { get; set; }
        public string EquipoPropio { get; set; }
        public string EquipoRenta { get; set; }
        public string FechaSolicitud { get; set; }
        public string FechaRequerida { get; set; }
        public string Estatus { get; set; }
        public string economicoAsignado { get; set; }
    }
}
