using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.KPI
{
    public class tblM_KPI_KPICapturaBit
    {
        public int id { get; set; }
        public int semana { get; set; }
        public int año { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public string kpiSemana { get; set; }
        public authEstadoEnum authEstado { get; set; }
        public int usuarioCaptura { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int idAutoriza { get; set; }
        public string ac { get; set; }
        public bool validado { get; set; }
    }
}
