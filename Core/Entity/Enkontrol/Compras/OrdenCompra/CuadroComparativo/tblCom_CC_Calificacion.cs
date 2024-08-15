using Core.Entity.Principal.Configuraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo
{
    /// <summary>
    /// Calificaciones de confiabilidad por proveedor
    /// </summary>
    public class tblCom_CC_Calificacion : RegistroDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// Numero de requisicion
        /// </summary>
        public int Numero { get; set; }
        public string CC { get; set; }
        public int Folio { get; set; }
        public int TipoRequisicion { get; set; }
        public int Proveedor { get; set; }
        public decimal Calificacion { get; set; }
        public decimal Precio { get; set; }
        public decimal TiempoEntrega { get; set; }
        public decimal CondicionPago { get; set; }
        public decimal LAB { get; set; }
        public decimal ConfiabilidadProveedor { get; set; }
        public decimal Calidad { get; set; }
        public decimal ServicioPostVenta { get; set; }
        public decimal PonderacionPrecio { get; set; }
        public decimal PonderacionTiempoEntrega { get; set; }
        public decimal PonderacionCondicionPago { get; set; }
        public decimal PonderacionLAB { get; set; }
        public decimal PonderacionConfiabilidadProveedor { get; set; }
        public decimal PonderacionCalidad { get; set; }
        public decimal PonderacionServicioPostVenta { get; set; }

        [ForeignKey("idCalificacion")]
        public virtual List<tblCom_CC_CalificacionPartida> partidas { get; set; }
    }
}
