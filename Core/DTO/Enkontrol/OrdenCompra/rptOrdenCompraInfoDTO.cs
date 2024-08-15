using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Enkontrol.OrdenCompra
{
    public class rptOrdenCompraInfoDTO
    {
        public rptOrdenCompraInfoDTO()
        {
            lstRetencionesDTO = new List<RetencionInfoDTO>();
            totalRetencion = "0";
            totalFinal = "0";
        }

        public string folioOrdenCompra { get; set; }
        public string fechaHoy { get; set; }
        public string provNumero { get; set; }
        public string provNombre { get; set; }
        public string provLugar { get; set; }
        public string provTelefono { get; set; }
        public string provFax { get; set; }
        public string labNumero { get; set; }
        public string labDescripcion { get; set; }
        public string cc { get; set; }
        public string ccDescripcion { get; set; }
        public string compradorNumero { get; set; }
        public string compradorNombre { get; set; }
        public string fechaOrdenCompra { get; set; }
        public string folioRequisicion { get; set; }
        public string comentarios { get; set; }
        public string facturar { get; set; }
        public string direccion { get; set; }
        public string rfc { get; set; }
        public string embarquese { get; set; }
        public string subTotal { get; set; }
        public string iva { get; set; }
        public string total { get; set; }
        public string elaboro { get; set; }
        public string reviso { get; set; }
        public string autorizo { get; set; }
        public string CFDI { get; set; }
        public string tipoCompra { get; set; }
        public string PERU_formaPago { get; set; }
        public string PERU_tipoCompra { get; set; }

        public string fechaVencimientoString { get; set; }

        public List<rptOrdenCompraFormaPagoDTO> pago { get; set; }
        public List<rptOrdenCompraPartidasDTO> partidas { get; set; }
        public List<dynamic> lstRetencionesEK { get; set; }
        public List<RetencionInfoDTO> lstRetencionesDTO { get; set; }
        public string totalRetencion { get; set; }
        public string totalFinal { get; set; }
    }
}
