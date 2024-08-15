using Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra.CuadroComparativo
{
    public class tblCom_CC_CalificacionPartida
    {
        public int id { get; set; }
        public int idCalificacion { get; set; }
        public int numeroPartida { get; set; }
        public int idTipoCalificacionPartida { get; set; }
        public decimal calificacion { get; set; }
        public decimal precio { get; set; }
        public decimal tiempoEntrega { get; set; }
        public decimal condicionPago { get; set; }
        public decimal LAB { get; set; }
        public decimal confiabilidadProveedor { get; set; }
        public decimal calidad { get; set; }
        public decimal servicioPostVenta { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool esActivo { get; set; }

        [ForeignKey("idCalificacion")]
        public virtual tblCom_CC_Calificacion calificacionProv { get; set; }
    }
}
