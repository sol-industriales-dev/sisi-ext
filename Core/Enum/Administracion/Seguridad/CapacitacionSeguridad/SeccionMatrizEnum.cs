using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.CapacitacionSeguridad
{
    public enum SeccionMatrizEnum
    {
        [DescriptionAttribute("Personal Activo")]
        PERSONAL_ACTIVO = 1,
        [DescriptionAttribute("Estadísticas Individuales")]
        ESTADISTICAS_INDIVIDUALES = 2,
        [DescriptionAttribute("Mando Administrativo Protocolo Fatalidad")]
        MANDO_ADMINISTRATIVO_PROTOCOLO_FATALIDAD = 3,
        [DescriptionAttribute("Mando Medio Protocolo Fatalidad")]
        MANDO_MEDIO_PROTOCOLO_FATALIDAD = 4,
        [DescriptionAttribute("Mando Operativo Protocolo Fatalidad")]
        MANDO_OPERATIVO_PROTOCOLO_FATALIDAD = 5,
        [DescriptionAttribute("Mando Administrativo Normativo")]
        MANDO_ADMINISTRATIVO_NORMATIVO = 6,
        [DescriptionAttribute("Mando Medio Normativo")]
        MANDO_MEDIO_NORMATIVO = 7,
        [DescriptionAttribute("Mando Operativo Normativo")]
        MANDO_OPERATIVO_NORMATIVO = 8,
        [DescriptionAttribute("Mando Administrativo Técnico Operativo")]
        MANDO_ADMINISTRATIVO_TECNICO_OPERATIVO = 9,
        [DescriptionAttribute("Mando Medio Técnico Operativo")]
        MANDO_MEDIO_TECNICO_OPERATIVO = 10,
        [DescriptionAttribute("Mando Operativo Técnico Operativo")]
        MANDO_OPERATIVO_TECNICO_OPERATIVO = 11,
        [DescriptionAttribute("Mando Administrativo Instructivo Operativo")]
        MANDO_ADMINISTRATIVO_INSTRUCTIVO_OPERATIVO = 12,
        [DescriptionAttribute("Mando Medio Instructivo Operativo")]
        MANDO_MEDIO_INSTRUCTIVO_OPERATIVO = 13,
        [DescriptionAttribute("Mando Operativo Instructivo Operativo")]
        MANDO_OPERATIVO_INSTRUCTIVO_OPERATIVO = 14
    }
}
