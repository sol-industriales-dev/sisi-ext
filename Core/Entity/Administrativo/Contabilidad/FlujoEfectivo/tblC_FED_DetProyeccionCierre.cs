using Core.Enum.Administracion.FlujoEfectivo;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.FlujoEfectivo
{
    public class tblC_FED_DetProyeccionCierre
    {
        public int id { get; set; }
        public int idConceptoDir { get; set; }
        public tipoProyeccionCierreEnum tipo { get; set; }
        public string ac { get; set; }
        public string cc { get; set; }
        public int anio { get; set; }
        public int semana { get; set; }
        public string descripcion { get; set; }
        public decimal monto { get; set; }
        public naturalezaEnum naturaleza { get; set; }
        public DateTime fecha { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int numcte { get; set; }
        public int numpro { get; set; }
        public int factura { get; set; }
        public DateTime fechaFactura { get; set; }
        public string grupo { get; set; }
        public int idDetProyGemelo { get; set; }
        [NotMapped]
        public EmpresaEnum empresa { get; set; }
        public int? grupoID { get; set; }
    }
}
