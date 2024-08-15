using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Rentabilidad;

namespace Core.DTO.Maquinaria.Reporte.Rentabilidad
{
    public class DetallesSemanalDTO
    {
        public string cc { get; set; }
        public List<DetallesKubrixPorCtaDTO> entrantes { get; set; }
        public List<DetallesKubrixPorCtaDTO> eliminados { get; set; }
    }
}
