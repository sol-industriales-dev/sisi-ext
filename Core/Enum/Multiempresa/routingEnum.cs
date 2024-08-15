using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Multiempresa
{
    public enum routingEnum : int
    {
        [DescriptionAttribute("local")]
        LOCAL = 1,
        [DescriptionAttribute("interna")]
        INTERNA = 2,
        [DescriptionAttribute("publica")]
        PUBLICA = 3
    }
}
