using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class SolicitudEquipoDTO
    {

        public int id { get; set; }
        public string Folio { get; set; }
        public int TipoId { get; set; }
        public int Grupoid { get; set; }
        public int Modeloid { get; set; }
        public string Tipo { get; set; }
        public string Grupo { get; set; }
        public string Modelo { get; set; }
        public string Descripcion { get; set; }
        public string pFechaInicio { get; set; }
        public string pFechaFin { get; set; }
        public int pHoras { get; set; }
        public string pFechaObra { get; set; }
        public string pTipoPrioridad { get; set; }
        public int pCapacidad { get; set; }
        public string Economico { get; set; }
        public string EconomicoRenta { get; set; }
        public int SolicitudDetalleId { get; set; }
        public int cantidadGrupo { get; set; }
        public int cantidadModelo { get; set; }
        public bool estatus { get; set; }
        public int HorasTotales { get; set; }
        public int tipoUtilizacion { get; set; }
        public string FechaPromesa { get; set; }
        public int idNoEconomico { get; set; }
        public string  Comentario {get;set;}
        public int tipoAsignacion { get; set; }
        public bool arranqueObra { get; set; }
        public string meses { get; set; }
        public string condicionInicial { get; set; }
        public string condicionActual { get; set; }
        public string justificacion { get; set; }
        public string link { get; set; }
    }
}
