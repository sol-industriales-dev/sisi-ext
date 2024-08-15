using Core.Enum.Administracion.FlujoEfectivo;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.FlujoEfectivo
{
    public class BusqFlujoEfectivoDetDTO
    {
        public tipoDetalleEnum tipo { get; set; }
        public int idConceptoDir { get; set; }
        public DateTime fechaPlaneacion { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string cc { get; set; }
        public string ac { get; set; }
        public string concepto { get; set; }
        public int numpro { get; set; }
        public int numcte { get; set; }
        public tipoProyeccionCierreEnum tipoCierre { get; set; }
        public tipoDetallePlaneacionEnum tipoPlan { get; set; }
        public EmpresaEnum empresa { get; set; }
        public bool esAnterior { get; set; }
    }
}
