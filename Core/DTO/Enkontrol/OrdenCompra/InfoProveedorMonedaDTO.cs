using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Principal.Generales;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class InfoProveedorMonedaDTO
    {
        public string id { get; set; }
        public string label { get; set; }
        public int moneda { get; set; }
        public string monedaDesc { get; set; }
        public decimal monedaTipoCambio { get; set; }
        public string cancelado { get; set; }
        public bool proveedorSubcontratistaBloqueado { get; set; }
        public bool proveedorSubcontratistaExiste { get; set; }
        public List<ComboDTO> listaCuentasCorrientes { get; set; }
        public string PERU_formaPago { get; set; }
        public string PERU_cuentaCorriente { get; set; }
    }
}
