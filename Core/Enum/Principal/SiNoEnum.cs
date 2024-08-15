using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal
{
    public enum SiNoEnum : int
    {
        [DescriptionAttribute("Si")]
        SI = 1,
        [DescriptionAttribute("No")]
        NO = 2
    }
}
