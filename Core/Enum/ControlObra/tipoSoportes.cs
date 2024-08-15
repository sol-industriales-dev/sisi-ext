using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
    public enum tipoSoportes
    {
        [DescriptionAttribute("Alcances")]
        Alcances = 1,
        [DescriptionAttribute("Modificacion")]
        Modificacion = 2,
        [DescriptionAttribute("Req")]
        Req = 3,
        [DescriptionAttribute("Ajuste")]
        Ajuste = 4,
        [DescriptionAttribute("Serv")]
        Serv = 5,
    }
}
