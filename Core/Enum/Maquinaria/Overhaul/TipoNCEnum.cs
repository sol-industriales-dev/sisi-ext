using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.NewFolder1
{
    public enum TipoNCEnum
    {
        [DescriptionAttribute("Reparación de Componentes")]
        NotaCredito = 1,
        [DescriptionAttribute("Casco Reman")]
        CascoReman = 2
    }
}
