using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.TransferenciasBancarias;

namespace Core.DTO.Administracion.TransferenciasBancarias
{
    public class RegistroArchivoDTO
    {
        public int numpro { get; set; }
        public decimal monto { get; set; }
        public string cuentaOrigen { get; set; }
        public string clabeOrigen { get; set; }
        public int bancoOrigen { get; set; }
        public string bancoOrigenDesc { get; set; }
        public string cuentaDestino { get; set; }
        public string clabeDestino { get; set; }
        public int bancoDestino { get; set; }
        public string bancoDestinoDesc { get; set; }
        public string referencia { get; set; }
        public string descripcion { get; set; }
        public OperacionEnum operacion { get; set; }
    }
}
