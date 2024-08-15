using Core.Enum.Contabilidad.EstadoFinanciero;
using Core.Enum.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.EstadoFinanciero
{
    public class BalanceDTO
    {
        public string concepto { get; set; }
        public TipoDetalleEnum tipoDetalle { get; set; }
        public decimal corte { get; set; }
        public decimal corteAnterior { get; set; }
        public decimal variacion { get; set; }
        public decimal dolares { get; set; }
        public bool renglonSubTitulo { get; set; }
        public bool renglonGrupo { get; set; }
        public bool renglonEnlace { get; set; }
        public bool sumarTotal { get; set; }

        public EmpresaEnum? empresa { get; set; }
        public int mes { get; set; }
        public decimal activoCirculante { get; set; }
        public decimal pasivoCirculante { get; set; }
        public decimal pasivoTotal { get; set; }
        public decimal capitalContableCirculante { get; set; }
        public decimal inventario { get; set; }
        public decimal activoTotal { get; set; }
    }
}
