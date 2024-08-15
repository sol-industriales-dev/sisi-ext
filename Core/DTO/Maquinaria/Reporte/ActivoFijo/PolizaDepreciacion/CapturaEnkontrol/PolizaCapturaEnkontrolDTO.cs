using Core.Entity.Maquinaria.Reporte.ActivoFijo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo.PolizaDepreciacion.CapturaEnkontrol
{
    public class PolizaCapturaEnkontrolDTO
    {
        public PolizaEnkontrolDTO Poliza { get; set; }
        public List<PolizaDetalleEnkontrolDTO> Detalle { get; set; }
        
        public List<int> idsEnvioCosto { get; set; }
        public tblC_AF_EnviarCosto EnviarCosto { get; set; }

        public PolizaCapturaEnkontrolDTO()
        {
            Poliza = new PolizaEnkontrolDTO();
            Detalle = new List<PolizaDetalleEnkontrolDTO>();
            idsEnvioCosto = new List<int>();
        }
    }
}