using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Contabilidad.ControlPresupuestal
{
    public enum ConceptoEnum
    {
        [DescriptionAttribute("Depreciación")]
        depreciacion = 1,
        [DescriptionAttribute("Seguro")]
        seguro = 2,
        [DescriptionAttribute("Filtros")]
        filtros = 3,
        [DescriptionAttribute("Correctivo")]
        correctivo = 4,
        [DescriptionAttribute("Depreciación Overhaul")]
        depreciacionOverhaul = 5,
        [DescriptionAttribute("Aceite")]
        aceite = 6,
        [DescriptionAttribute("Carrilería")]
        carrileria = 7,
        [DescriptionAttribute("Ansul")]
        ansul = 8,
        [DescriptionAttribute("Otros")]
        otros = 9,
        [DescriptionAttribute("Daños")]
        danos = 10
    }
}
