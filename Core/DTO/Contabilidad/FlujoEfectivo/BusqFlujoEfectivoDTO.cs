using Core.Enum.Administracion.FlujoEfectivo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class BusqFlujoEfectivoDTO
    {
        public tipoBusqueda tipo { get; set; }
        public DateTime min { get; set; }
        public DateTime max { get; set; }
        public List<string> lstCC { get; set; }
        public List<string> lstAC { get; set; }
        public List<int> lstTm { get; set; }
        public int idConcepto { get; set; }
        public bool esFlujo { get; set; }
        public bool esCC { get; set; }
        public bool esConciliado { get; set; }
    }
}
