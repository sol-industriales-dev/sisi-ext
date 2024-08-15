using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class ComparativoDTO
    {
        public int id { get; set; }
        public int idAsignacion { get; set; }
        public string marcaModelo { get; set; }
        public int estatus { get; set; }
        public int estatusFinanciera { get; set; }
        public int idMegusta { get; set; }

        public int idDet { get; set; }
        public int idComparativo { get; set; }
        public int idRow { get; set; }
        public string proveedor { get; set; }
        public string precioDeVenta { get; set; }
        public string tradeIn { get; set; }
        public string valoresDeRecompra { get; set; }
        public string precioDeRentaPura { get; set; }
        public string precioDeRentaEnRoc { get; set; }
        public string baseHoras { get; set; }
        public string tiempoDeEntrega { get; set; }
        public string ubicacion { get; set; }
        public string horas { get; set; }
        public string seguro { get; set; }
        public string garantia { get; set; }
        public string serviciosPreventivos { get; set; }
        public string capacitacion { get; set; }
        public string depositoEnGarantia { get; set; }
        public string lugarDeEntrega { get; set; }
        public string flete { get; set; }
        public string condicionesDePagoEntrega { get; set; }
        public string caracteristicasDelEquipo { get; set; }


        public string financiera { get; set; }
        public bool esActivo { get; set; }


        public int idFinanciero { get; set; }
      
        public string tipoDeArrendamiento { get; set; }
        public string tasa { get; set; }
        public string enganche { get; set; }
        public string opcionDeCompra { get; set; }
        public string monto { get; set; }
        public string rentaFija { get; set; }
        public string interes { get; set; }
        public string comisionMonto { get; set; }
        public string opcionDeCompraMonto { get; set; }
        public string EngancheMonto { get; set; }
        public string depositoEnGarantiaMonto { get; set; }
        
        public string header { get; set; }
        public string txtIdnumero1 { get; set; }
        public string txtIdnumero2 { get; set; }
        public string txtIdnumero3 { get; set; }
        public string txtIdnumero4 { get; set; }
        public string txtIdnumero5 { get; set; }
        public string txtIdnumero6 { get; set; }
        public string txtIdnumero7 { get; set; }
        public string txtIdnumero8 { get; set; }
        public string txtIdnumero9 { get; set; }
        public string txtIdnumero10 { get; set; }
        public string txtIdnumero11 { get; set; }
        public string txtIdnumero12 { get; set; }

        public int estatusExito { get; set; }
        public string msjExito { get; set; }
        public List<tblM_ComparativosAdquisicionyRentaCaracteristicasDet> lstCaracteristicas { get; set; }

        public int numeroMayor { get; set; }
        public bool check { get; set; }
        public int idComparativoDetalle { get; set; }
        public int votoMayor { get; set; }
        public string voto { get; set; }

        public int autorizanteID { get; set; }
        public string autorizanteNombre { get; set; }
        public string autorizantePuesto { get; set; }
        public bool autorizanteStatus { get; set; }
        public bool autorizanteFinal { get; set; }
        public DateTime ?autorizanteFecha { get; set; }
        public string firma { get; set; }
        public string tipo { get; set; }
        public int orden { get; set; }
        public string comentario { get; set; }
        public string TipoSolicitud { get; set; }
        public string folioAdquisicion { get; set; }
        public string folioFinanciera { get; set; }
        public string plazosFin { get; set; }
        public string ComentarioGeneral { get; set; }
        public DateTime? fechaDeElaboracion { get; set; }
        public DateTime? fechaDeElaboracionFinanciero { get; set; }


        public string obra { get; set; }
        public string nombreDelEquipo { get; set; }
        public bool compra { get; set; }
        public bool renta { get; set; }
        public bool roc { get; set; }

        public string banco { get; set; }
        public string plazo { get; set; }
        public string precioDelEquipo {get;set;}
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

        public string caracteristicasDelEquipo1 { get; set; }
        public string caracteristicasDelEquipo2 { get; set; }
        public string caracteristicasDelEquipo3 { get; set; }
        public string caracteristicasDelEquipo4 { get; set; }
        public string caracteristicasDelEquipo5 { get; set; }
        public string caracteristicasDelEquipo6 { get; set; }
        public string caracteristicasDelEquipo7 { get; set; }
        public string rutaArchivo { get; set; }

        public string classColor { get; set; }
        public string tipoMoneda { get; set; }
        public int idCuadro { get; set; }
    }
}
