using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class TablaReporteEjecutivoDTO
    {
        public string cc { get; set; }
        public decimal clasificar { get; set; }
        public decimal ordenar { get; set; }
        public decimal limpiar { get; set; }
        public decimal estandarizar { get; set; }
        public decimal disciplina { get; set; }
        public decimal total { get; set; }
    }
}
