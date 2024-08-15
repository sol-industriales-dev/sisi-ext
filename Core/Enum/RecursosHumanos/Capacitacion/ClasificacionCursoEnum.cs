using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Core.Enum.RecursosHumanos.Capacitacion
{
    public enum ClasificacionCursoEnum
    {
        [DescriptionAttribute("Protocolo de fatalidad")]
        ProtocoloFatalidad = 1,
        [DescriptionAttribute("Normativo")]
        Normativo = 2,
        [DescriptionAttribute("General")]
        General = 3,
        [DescriptionAttribute("Formativo Normativo")]
        Formativo = 4,
        [DescriptionAttribute("Instructivo Operativo")]
        InstructivoOperativo = 5,
        [DescriptionAttribute("Técnico Operativo")]
        TecnicoOperativo = 6,
        [DescriptionAttribute("Mandos Medios")]
        MandosMedios = 7,
        [DescriptionAttribute("Habilidades Técnicas")]
        HabilidadesTécnicas = 8,
        [DescriptionAttribute("Habilidades Blandas")]
        HabilidadesBlandas = 9,
        [DescriptionAttribute("Inductivo")]
        Inductivo = 10
    }
}
