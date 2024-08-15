using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_ComparativoFinancieroDet
    {
        public int id { get; set; }
        public int idFinanciero { get; set; }
        public int idRow { get; set; }
        public string banco { get; set; }
        public string plazo { get; set; }
        public string precioDelEquipo { get; set; }
        public string tiempoRestanteProyecto { get; set; }
        public string iva { get; set; }
        public string total { get; set; }
        public string montoFinanciar { get; set; }
        public string tipoOperacion { get; set; }
        public string opcionCompra { get; set; }
        public string valorResidual { get; set; }
        public string depositoEfectivo { get; set; }
        public string moneda { get; set; }
        public string plazoMeses { get; set; }
        public string tasaDeInteres { get; set; }
        public string gastosFijos { get; set; }
        public string comision { get; set; }
        public string montoComision { get; set; }
        public string rentasEnGarantia { get; set; }
        public string crecimientoPagos { get; set; }
        public string pagoInicial { get; set; }
        public string pagoTotalIntereses { get; set; }
        public string tasaEfectiva { get; set; }
        public string mensualidad { get; set; }
        public string mensualidadSinIVA { get; set; }
        public string pagoTotal { get; set; }

    }
}
