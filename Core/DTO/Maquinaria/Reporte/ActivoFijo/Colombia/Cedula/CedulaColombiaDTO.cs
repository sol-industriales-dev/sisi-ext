using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.Colombia.Cedula
{
    public class CedulaColombiaDTO
    {
        public CedulaSaldoColombiaDTO saldo { get; set; }
        public CedulaDepColombiaDTO dep { get; set; }
        public List<CedulaDetalleColombiaDTO> detalle { get; set; }
    }
}
