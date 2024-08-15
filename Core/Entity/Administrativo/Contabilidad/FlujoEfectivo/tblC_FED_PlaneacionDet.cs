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
    public class tblC_FED_PlaneacionDet
    {
        public int id { get; set; }
        public int concepto { get; set; }
        public string descripcion { get; set; }
        public decimal monto { get; set; }
        public string ac { get; set; }
        public string cc { get; set; }
        public int semana { get; set; }
        public int año { get; set; }
        public bool estatus { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int usuarioCaptura { get; set; }
        public int sp_gastos_provID { get; set; }
        public string factura { get; set; }
        public int nominaID { get; set; }
        public int cadenaProductivaID { get; set; }
        public int numcte { get; set; }
        public int numprov { get; set; }
        public string fechaFactura { get; set; }
        public int idDetProyGemelo { get; set; }
        [NotMapped]
        public EmpresaEnum empresa { get; set; }
        [NotMapped]
        public tipoDetallePlaneacionEnum tipo { get; set; }
        public int categoriaTipo { get; set; }
    }
}
