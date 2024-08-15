using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Subcontratistas;
using Core.Enum.ControlObra;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.SubContratistas
{
    public class tblX_Proyecto
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string cc { get; set; }
        public string numeroContrato { get; set; }
        public AreasEnum area { get; set; }
        public DateTime? fechaSuscripcion { get; set; }
        public DateTime? fechaVigencia { get; set; }
        public decimal montoContractual { get; set; }
        public bool anticipoAplica { get; set; }
        public decimal anticipoPorcentaje { get; set; }
        public string penalizacion { get; set; }
        public int clienteID { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("proyectoID")]
        public virtual List<tblX_Contrato> contratos { get; set; }
    }
}
