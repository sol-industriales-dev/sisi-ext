using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class incidenciasPresentadasDTO
    {
        public string centroCosto { get; set; }
        public int cantidadFatal { get; set; }
        public int cantidadLTA { get; set; }
        public int cantidadATR { get; set; }
        public int cantidadATM { get; set; }
        public int cantidadAPA { get; set; }
        public int cantidadDAMEQ { get; set; }
        public int cantidadNM { get; set; }
        public int cantidadEI { get; set; }
        public int cantidadOI { get; set; }
        public decimal cantidadHH { get; set; }

        public int cantidadFatalIndicador { get; set; }
        public int cantidadLTAIndicador { get; set; }
        public int cantidadATRIndicador { get; set; }
        public int cantidadATMIndicador { get; set; }
        public int cantidadAPAIndicador { get; set; }
        public int cantidadDAMEQIndicador { get; set; }
        public int cantidadNMIndicador { get; set; }
        public int cantidadOIindicador { get; set; }
        public int cantidadEIindicador { get; set; }
        public int cantidadTotalIndicador { get; set; }
        public decimal cantidadMaxIndicador { get; set; }
        public decimal cantidadMinIndicador { get; set; }

        public int cantidadTotalTipo { get; set; }

        public decimal LesionesRegistrables { get; set; }
        public decimal LesionesIncapacitantes { get; set; }
        public decimal LesionesTotales { get; set; }
        public decimal LesionesDanios { get; set; }
        public decimal severidad { get; set; }
        public decimal horasHombre { get; set; }
        public decimal LTIFR { get; set; }
        public decimal TRIFR { get; set; }
        public decimal TIFR { get; set; }
        public decimal IFA { get; set; }
        public decimal ISA { get; set; }
        public decimal IA { get; set; }
        public decimal lostDays { get; set; }
        public string orden { get; set; }
    }
}
