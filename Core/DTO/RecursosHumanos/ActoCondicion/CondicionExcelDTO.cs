using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.ActoCondicion
{
    public class CondicionExcelDTO
    {
        public string folio { get; set; }
        public string proyecto { get; set; }
        public string descripcion { get; set; }
        public string fechaSuceso { get; set; }
        public string fechaResolucion { get; set; }

        public string departamento { get; set; }
        public string clasificacion { get; set; }
        public string procedimiento { get; set; }

        public DateTime fechaSucesoDT { get; set; }
        public DateTime? fechaResolucionDT { get; set; }
        public int subClasificacionDepID { get; set; }
        public string subclasificacion { get; set; }
        public string empleadoInformo { get; set; }
    }
}
