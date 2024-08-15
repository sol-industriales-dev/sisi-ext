using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum ordenesEnum
    {
        [DescriptionAttribute("Autorizada")]
        Autorizada = 1,
        [DescriptionAttribute("Rechazada")]
        Rechazada = 2,
        [DescriptionAttribute("Pendiente")]
        Pendiente = 3,
        [DescriptionAttribute("Autorizada Anterior")]
        AutorizadaAnterior = 4,
        [DescriptionAttribute("Pendiente por autorizar")]
        PendientePorAutorizar = 5,
        [DescriptionAttribute("Pendiente Vobo 1")]
        PendienteVobo1 = 6,
        [DescriptionAttribute("Pendiente Vobo 2")]
        PendienteVobo2 = 7,
        [DescriptionAttribute("VoBo Realizado")]
        VoBoRealizado = 8
    }
}
