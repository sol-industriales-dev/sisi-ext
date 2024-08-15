using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria
{
    public class FichaTecnicaDTO
    {
        public string noEconomico {get;set;}

        public string descripcion {get;set;}
        public string marca {get;set;}
        public string modelo {get;set;}
        public string noSerie {get;set;}
        public string anio {get;set;}
        public string fechaCompra {get;set;}
        public string horometroInicio {get;set;}
        public string ubicacion { get; set; }
        public string horometroActual {get;set;}
        public string fechaParo {get;set;}
        public string detParo {get;set;}
        public string costoAdquisicion {get;set;}
        public string costoOverHaul {get;set;}
        public string costoOverHaulAplicado { get; set; }

    }
}
