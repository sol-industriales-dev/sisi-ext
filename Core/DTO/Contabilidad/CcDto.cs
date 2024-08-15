using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class CcDTO
    {
        public string cc { get; set; }
        public string descripcion { get; set; }
        public String bit_area { get; set; }
        public String st_ppto { get; set; }
        public Nullable<DateTime> fecha_registro { get; set; }
        public int ordernFlujoEfectivo { get; set; }
        public DateTime inicioObra { get; set; }
        public CcDTO()
        {
            inicioObra = fecha_registro ?? new DateTime(2016, 06, 27);
        }
    }
}
