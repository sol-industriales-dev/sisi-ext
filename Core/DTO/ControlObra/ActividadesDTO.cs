using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra
{
    public class ActividadesDTO
    {
        public int id { get; set; }
        public string actividad { get; set; }         
        public decimal cantidad { get; set; }
        public decimal? precioUnitario { get; set; }
        public decimal? importeContratado { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public bool estatus { get; set; }
        public bool actividadPadreRequerida { get; set; }
        public bool actividadTerminada { get; set; }
        public int? subcapituloN3_id { get; set; }
        public string subcapituloN3 { get; set; }
        public int? subcapituloN2_id { get; set; }
        public string subcapituloN2 { get; set; }
        public int? subcapituloN1_id { get; set; }
        public string subcapituloN1 { get; set; }
        public int? capitulo_id { get; set; }
        public string capitulo { get; set; }
        public int? unidad_id { get; set; }
        public string unidad   { get; set; }    
        public int? actividadPadre_id { get; set; }
        public string actividadPadre { get; set; }
        public int? tipoPeriodoAvance { get; set; }
       
    }
}
