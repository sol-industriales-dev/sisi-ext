using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar.Poliza_revaluacion
{
    public class PolizaRevaluacionDetalleDTO
    {
        public string proveedor { get; set; }
        public string contrato { get; set; }
        public string activo { get; set; }
        public decimal tipoCambio { get; set; }
        public decimal deudaCP { get; set; }
        public decimal valuacionCP { get; set; }
        public decimal contabilidadCP { get; set; }
        public decimal diferenciaCP { get; set; }
        public decimal deudaLP { get; set; }
        public decimal valuacionLP { get; set; }
        public decimal contabilidadLP { get; set; }
        public decimal diferenciaLP { get; set; }
        public decimal gananciaPerdidaCambiaria { get; set; }

        public int ctaCP { get; set; }
        public int sctaCP { get; set; }
        public int ssctaCP { get; set; }
        public int digitoCP { get; set; }

        public int ctaLP { get; set; }
        public int sctaLP { get; set; }
        public int ssctaLP { get; set; }
        public int digitoLP { get; set; }

        public bool esPQ { get; set; }
        public string cc { get; set; }
        public string conceptoPQ { get; set; }
    }
}
