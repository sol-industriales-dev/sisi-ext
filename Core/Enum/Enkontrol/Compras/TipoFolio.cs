using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Enkontrol.Compras
{
    public enum TipoFolio
    {
        //[DescriptionAttribute("Orden Trabajo")]
        //OrdenTrabajo = 1,
        //[DescriptionAttribute("Stand By")]
        //StandBy = 2,
        //[DescriptionAttribute("Captura Horómetro")]
        //CapturaHorometro = 3,
        //[DescriptionAttribute("Nota Crédito")]
        //NotaCredito = 4
        [DescriptionAttribute("Mantenimiento P.M.")]
        MantenimientoPM = 1,
        [DescriptionAttribute("Overhaul")]
        Overhaul = 3,
        [DescriptionAttribute("Stock Sugerido")]
        OrigenStock = 2
    }
}
