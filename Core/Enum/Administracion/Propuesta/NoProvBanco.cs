using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Propuesta
{
    /// <summary>
    /// Número de proveedr del catálogo de sp_proveedores, únicamente los bancos en uso
    /// </summary>
    public enum NoProvBanco
    {
        [DescriptionAttribute("BANAMEX")]
        Banamex = 940,
        [DescriptionAttribute("BANORTE")]
        Banorte = 1179,
        [DescriptionAttribute("SANTANDER")]
        Santander = 3965,
        [DescriptionAttribute("MONEX")]
        Monex = 4392,
        [DescriptionAttribute("BANAMEX DLL")]
        BanamexDll = 9157,
        [DescriptionAttribute("BANORTE DLL")]
        BanorteDll = 9676,
    }
}
