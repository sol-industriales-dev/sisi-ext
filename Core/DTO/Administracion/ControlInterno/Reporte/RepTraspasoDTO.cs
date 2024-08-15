using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.ControlInterno.Reporte
{
    
    public class RepTraspasoDTO
    {
        public string folio { get; set; }
        public string mov52 { get; set; }
        public string mov2 { get; set; }
        public string almacen_origen { get; set; }
        public string almacen_destino { get; set; }
        public string numero52 { get; set; }
        public string numero2 { get; set; }
        public DateTime fecha_salida { get; set; }
        public DateTime fecha_entrada { get; set; }
        public DateTime fecha { get; set; }
        public string strfecha_salida { get; set; }
        public string strfecha_entrada { get; set; }
        public string strfecha { get; set; }
        public string cc { get; set; }
        public decimal total { get; set; }
        public decimal totalEntrada { get; set; }
        public decimal totalSalida { get; set; }
    }
}
