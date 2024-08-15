using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Encuesta
{
    public enum TiposPreguntasEnum
    {
        [DescriptionAttribute("(C) CALIDAD")]
        CALIDAD = 1,
        [DescriptionAttribute("(T) TIEMPO DE ENTREGA")]
        TIEMPO = 2,
        [DescriptionAttribute("(F) FACTURACION")]
        FACTURACION = 3,
        [DescriptionAttribute("(A) ATENCIÓN")]
        ATENCION = 4,
        [DescriptionAttribute("(P) PLANEACION/PROGRAMA")]
        PLANEACION = 5,
        [DescriptionAttribute("(S) SEGURIDAD")]
        SEGURIDAD = 6,
        [DescriptionAttribute("(A) AMBIENTAL")]
        AMBIENTAL = 7,
        [DescriptionAttribute("(E) EFECTIVIDAD DEL COSTO")]
        EFECTIVIDAD = 8,
        [DescriptionAttribute("(M) FUERZA DE TRABAJO")]
        FUERZATRABAJO = 9,
        [DescriptionAttribute("(O) OTROS")]
        OTROS = 10
    }
}
