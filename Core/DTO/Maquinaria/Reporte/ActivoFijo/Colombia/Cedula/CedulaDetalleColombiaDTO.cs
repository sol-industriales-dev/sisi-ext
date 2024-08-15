using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.Colombia.Cedula
{
    public class CedulaDetalleColombiaDTO
    {
        public int id { get; set; }
        public bool esMaquina { get; set; }
        public int? idActivo { get; set; }
        public string factura { get; set; }
        public string noEconomico { get; set; }
        public string descripcion { get; set; }
        public DateTime fechaMovimiento { get; set; }
        public DateTime fechaInicioDep { get; set; }
        public int mesesDepreciacion { get; set; }
        public decimal porcentajeDepreciacion { get; set; }
        public bool capturaAutomatica { get; set; }
        public string cc { get; set; }
        public string ccDescripcion { get; set; }
        public string ccCompleto { get; set; }
        public int polYear { get; set; }
        public int polMes { get; set; }
        public int polPoliza { get; set; }
        public string polTp { get; set; }
        public int polLinea { get; set; }
        public string polizaCompleta { get; set; }
        public int tmPolizaId { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string cuentaCompleta { get; set; }
        public decimal moi { get; set; }
        public decimal alta { get; set; }
        public DateTime fechaPoliza { get; set; }
        public decimal baja { get; set; }
        public DateTime? fechaBaja { get; set; }
        public int? tmPolizaId_Baja { get; set; }
        public decimal depMensual { get; set; }
        public int diasDepPrimerMes { get; set; }
        public int mesesDepreciadosAnteriormente { get; set; }
        public int mesesDepreciadosActualmente { get; set; }
        public decimal depreciacionAnterior { get; set; }
        public decimal depreciacionActual { get; set; }
        public decimal bajaDepreciacion { get; set; }
        public decimal depreciacionAcumulada { get; set; }
        public decimal saldoLibro { get; set; }
        public bool depTerminadaPorTiempo { get; set; }
    }
}
