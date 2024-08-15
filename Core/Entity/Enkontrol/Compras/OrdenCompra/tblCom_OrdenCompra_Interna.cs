using Core.Enum.Enkontrol;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras.OrdenCompra
{
    public class tblCom_OrdenCompra_Interna
    {
        #region SQL
        public int id { get; set; }
        public string cc { get; set; }
        public int numero { get; set; }
        public DateTime fecha { get; set; }
        public int? idLibreAbordo { get; set; }
        public string tipo_oc_req { get; set; }
        public int compradorSIGOPLAN { get; set; }
        public int compradorEnkontrol { get; set; }
        public string moneda { get; set; }
        public decimal tipo_cambio { get; set; }
        public decimal porcent_iva { get; set; }
        public decimal sub_total { get; set; }
        public decimal iva { get; set; }
        public decimal total { get; set; }
        public string estatus { get; set; }
        public string comentarios { get; set; }
        public string bienes_servicios { get; set; }
        public string CFDI { get; set; }
        public int tiempoEntregaDias { get; set; }
        public string tiempoEntregaComentarios { get; set; }
        public bool anticipo { get; set; }
        public decimal totalAnticipo { get; set; }
        public bool estatusRegistro { get; set; }
        public bool colocada { get; set; }
        public DateTime? colocadaFecha { get; set; }
        public string correoProveedor { get; set; }
        public int? proveedor { get; set; }
        public string st_impresa { get; set; }
        public int autorizo { get; set; }
        public int usuario_autoriza { get; set; }
        public DateTime? fecha_autoriza { get; set; }
        public string ST_OC { get; set; }
        public int empleado_autoriza { get; set; }
        public int empleadoUltimaAccion { get; set; }
        public DateTime? fechaUltimaAccion { get; set; }
        public TipoUltimaAccionEnum tipoUltimaAccion { get; set; }
        public int categoria { get; set; }
        public int vobo { get; set; }
        public DateTime? fechaVoBo1 { get; set; }
        public int vobo2 { get; set; }
        public DateTime? fechaVoBo2 { get; set; }
        public int vobo3 { get; set; }
        public DateTime? fechaVoBo3 { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONALES
        public virtual List<tblCom_OCRetenciones_Interna> retenciones { get; set; }
        [NotMapped]
        public virtual List<tblAlm_Movimientos> OC_Movimientos { get; set; }
        #endregion
    }
}
