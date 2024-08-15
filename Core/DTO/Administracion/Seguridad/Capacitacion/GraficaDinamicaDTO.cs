using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class GraficaDinamicaDTO
    {
        public List<string> categorias { get; set; }
        public List<string> seriesDescripciones { get; set; }
        public List<List<decimal>> seriesDatos { get; set; }       
        public string serieDescripcionObjetivoMinimo { get; set; }
        public new List<decimal> serieObjetivoMinimo { get; set; }
        public string lstMeses { get; set; }
        public string proyectos { get; set; }
        public string mes { get; set; }
        public decimal porcentajes { get; set; }
        public GraficaDinamicaDTO()
        {
            categorias = new List<string>();
            seriesDescripciones = new List<string>();
            seriesDatos = new List<List<decimal>>();
            serieObjetivoMinimo = new List<decimal>();
        }
    }
}
