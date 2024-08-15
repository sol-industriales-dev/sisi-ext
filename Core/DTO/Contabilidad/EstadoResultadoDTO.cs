using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Contabilidad;

namespace Core.DTO.Contabilidad
{
    public class EstadoResultadoDTO
    {
        public string concepto { get; set; }
        public TipoOperacionEnum tipoOperacion { get; set; }
        public int detalleID { get; set; }

        public decimal montoMesAnioActual { get; set; }
        public decimal montoMesAnioAnterior { get; set; }
        public decimal montoMesAcumuladoAnioActual { get; set; }
        public decimal montoMesAcumuladoAnioAnterior { get; set; }

        public decimal porcentajeMesAnioActual { get; set; }
        public decimal porcentajeMesAnioAnterior { get; set; }
        public decimal porcentajeMesAcumuladoAnioActual { get; set; }
        public decimal porcentajeMesAcumuladoAnioAnterior { get; set; }

        public decimal variaciones { get; set; }
        public decimal variacionesAcumulado { get; set; }

        public int grupoReporteID { get; set; }
        public int ordenReporte { get; set; }
        public bool flagGrupoReporte { get; set; }

        public List<EstadoResultadoPorEmpresaDTO> resultadosPorEmpresa { get; set; }
        public decimal montoConsolidado { get; set; }
        public decimal porcentajeConsolidado { get; set; }
        public decimal montoConsolidadoYearAnterior { get; set; }
        public decimal porcentajeConsolidadoYearAnterior { get; set; }

        public decimal montoClienteProveedor { get; set; }
        public decimal montoClienteProveedorAnterior { get; set; }

        public EstadoResultadoDTO()
        {
            resultadosPorEmpresa = new List<EstadoResultadoPorEmpresaDTO>();
        }
    }
}
