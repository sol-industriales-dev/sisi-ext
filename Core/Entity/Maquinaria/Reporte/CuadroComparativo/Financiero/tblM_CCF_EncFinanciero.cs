using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Principal.Configuraciones;
using Core.Enum.Administracion.Cotizaciones;
using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Reporte.CuadroComparativo.Financiero
{
    public class tblM_CCF_EncFinanciero : RegistroDTO
    {
        public int Id { get; set; }
        public int IdAsignacion { get; set; }
        public decimal Renta { get; set; }
        public TipoMonedaEnum IdMoneda { get; set; }
        public decimal RentaMes { get; set; }
        public DateTime FechaElaboracion { get; set; }
        public string Comentario { get; set; }
        public int IdFinanciero { get; set; }
        public authEstadoEnum Estado { get; set; }
        [ForeignKey("IdCuadro")]
        public virtual IEnumerable<tblM_CCF_DetFinanciero> Detalles { get; set; }
        [ForeignKey("IdCuadro")]
        public virtual IEnumerable<tblM_CCF_Auth> Autorizantes { get; set; }
    }
}
