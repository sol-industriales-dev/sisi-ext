using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.Maquinaria.Inventario
{
    public class ComparativoAdquisicion
    {
        public int idDet { get; set; }
        public int idRow { get; set; }
        public int idComparativo { get; set; }
        public string marcaModelo { get; set; }
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
        public HttpPostedFileBase file { get; set; }
        public List<tblM_ComparativosAdquisicionyRentaCaracteristicasDet> lstCaracteristicas { get; set; }

        public string caracteristicasDelEquipo1 { get; set; }
        public string caracteristicasDelEquipo2 { get; set; }
        public string caracteristicasDelEquipo3 { get; set; }
        public string caracteristicasDelEquipo4 { get; set; }
        public string caracteristicasDelEquipo5 { get; set; }
        public string caracteristicasDelEquipo6 { get; set; }
        public string caracteristicasDelEquipo7 { get; set; }
        public string tipoMoneda { get; set; }
        public string comentarios { get; set; }
    }
}
