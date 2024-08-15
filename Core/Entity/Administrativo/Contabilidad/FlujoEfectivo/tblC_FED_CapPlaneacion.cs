
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.FlujoEfectivo
{
    public class tblC_FED_CapPlaneacion
    {
        public int id { get; set; }
        public int idConceptoDir { get; set; }
        public string ac { get; set; }
        public string cc { get; set; }
        public DateTime fecha { get; set; }
        public int anio { get; set; }
        public int semana { get; set; }
        public decimal planeado { get; set; }
        public decimal corte { get; set; }
         [NotMapped]
        public decimal flujoTotal { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string strFlujoEfectivo { get; set; }
        public string strSaldoInicial { get; set; }
        [NotMapped]
        public EmpresaEnum empresa { get; set; }
        public tblC_FED_CapPlaneacion()
        {
            var total = 0m;
            strFlujoEfectivo = strFlujoEfectivo ?? string.Empty;
            var esParceado = decimal.TryParse(strFlujoEfectivo.Replace(",", "").Replace(".", "").Replace("$", ""), NumberStyles.Number, CultureInfo.InvariantCulture,out total);
            if (esParceado)
            {
                this.flujoTotal = total;
            }
            else
            {
                this.flujoTotal = 0;
            }
        }
    }
}
