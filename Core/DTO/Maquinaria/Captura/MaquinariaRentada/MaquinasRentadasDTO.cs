using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura.MaquinariaRentada
{
    public class MaquinasRentadasDTO
    {
        public int Id { get; set; }
        public string Folio { get; set; }
        public string CentroCosto { get; set; }
        public string AreaCuenta { get; set; }
        public DateTime PeriodoDel { get; set; }
        public DateTime PeriodoA { get; set; }
        public decimal? TotalRenta { get; set; }
        public string OrdenCompra { get; set; }
        public bool RentaTerminada { get; set; }
        public bool DifHora { get; set; }
        public bool CargoDaño { get; set; }
        public bool Fletes { get; set; }
        public int IdTipoMoneda { get; set; }
        public int DiasParaTerminar
        {
            get
            {
                var _fechaActual = DateTime.Now;
                if (RentaTerminada) { return 999999999; } else { return (PeriodoA - _fechaActual).Days; }
            }
        }
    }
}