using Core.Enum.Contabilidad.EstadoFinanciero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad
{
    public class EstadoPosicionFinancieraDTO
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
        public bool esPasivo { get; set; }
    }
}
