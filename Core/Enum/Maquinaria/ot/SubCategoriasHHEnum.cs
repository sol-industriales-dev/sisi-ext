using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Maquinaria.ot
{
    public enum SubCategoriasHHEnum
    {
        [DescriptionAttribute("Talleres")]
        Talleres = 201,
        [DescriptionAttribute("Oficinas")]
        Oficinas = 202,
        [DescriptionAttribute("Equipo de transporte")]
        LIMPIEZA = 203,
        [DescriptionAttribute("Equipo menor")]
        EQUIPOMENOR = 204,
        [DescriptionAttribute("Reparación de componentes menores")]
        REPARACIONCOMPONETESMENORES = 205,
        [DescriptionAttribute("Otros")]
        OTROS = 206


    }
}
