using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.ControlPresupuestal
{
    public class FiltrosDashboardDTO
    {
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public List<int> listaDivisiones { get; set; }
        public List<int> listaProyectos { get; set; }
        public int tipo { get; set; }
        public int grupo { get; set; }
        public int modelo { get; set; }
        public List<int> listaModelos { get; set; }
        public List<int> listaConceptos { get; set; }
        public int TipoBusqueda { get; set; }
        public int FiltroBusqueda { get; set; }
    }
}
