using Core.Enum.Administracion.FlujoEfectivo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class BusqProyeccionCierreDTO
    {
        public DateTime min { get; set; }
        public DateTime max { get; set; }
        public List<string> lstAC { get; set; }
        public List<string> lstCC { get; set; }
        public int idConceptoDir { get; set; }
        public tipoProyeccionCierreEnum tipo { get; set; }
        public string grupo { get; set; }
        public bool esCC { get; set; }
    }
}
