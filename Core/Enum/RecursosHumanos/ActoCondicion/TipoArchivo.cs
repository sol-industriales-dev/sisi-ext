using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Core.Enum.RecursosHumanos.ActoCondicion
{
    public enum TipoArchivo
    {
        [DescriptionAttribute("Evidencia")]
        Evidencia = 1,
        [DescriptionAttribute("Imagen antes")]
        ImagenAntes = 2,
        [DescriptionAttribute("Imagen después")]
        ImagenDespues = 3
    }
}
