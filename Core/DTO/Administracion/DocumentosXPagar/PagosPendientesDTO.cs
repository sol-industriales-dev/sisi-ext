using Core.Entity.Administrativo.DocumentosXPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class PagosPendientesDTO
    {
        public int contratoid { get; set; }
        public int parcialidad { get; set; }
        public string rfc { get; set; }
        public string noEconomico { get; set; }
        public string mensualidad { get; set; }
        public string financiamiento { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public string contrato { get; set; }
        public decimal capital { get; set; }
        public decimal intereses { get; set; }
        public decimal iva { get; set; }
        public decimal ivaInteres { get; set; }
        public decimal importe { get; set; }
        public decimal importeDLLS { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal total { get; set; }
        public int programado { get; set; }
        public decimal porcentaje { get; set; }
        public string cc { get; set; }
        public string areaCuenta { get; set; }
        public string acDescripcion { get; set; }
        public int moneda { get; set; }
        public int empresa { get; set; }
        public decimal penaConvencional { get; set; }
        public decimal montoOpcionCompra { get; set; }
        public bool liquidar { get; set; }
        public bool opcionCompra { get; set; }

        public int maquinaId { get; set; }
        public string maquinaCentroCostos { get; set; }
        public tblAF_DxP_ContratoDetalle detalle { get; set; }
    }
}
