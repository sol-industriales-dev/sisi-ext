using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.ActoCondicion
{
    public class ReporteActoCondicionExcelDTO
    {
        public int folio { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaSuceso { get; set; }
        public DateTime? fechaCorreccion { get; set; }
        public int? numeroInfraccion { get; set; }
        public string rutaImagenAntes { get; set; }
        public string rutaImagenDespues { get; set; }
        public string accionCorrectiva { get; set; }
        public string nombreSupervisor { get; set; }
        public string nombreInformo { get; set; }
        public string nombre { get; set; }
        public string puesto { get; set; }

        public string tipoRiesgoDescripcion { get; set; }
        public string tipoActo { get; set; }
        public string nivelPrioridadDescripcion { get; set; }
        public string empresa { get; set; }
        public string descripcionInfraccion { get; set; }
    }
}
