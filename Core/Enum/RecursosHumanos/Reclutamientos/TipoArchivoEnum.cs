using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.RecursosHumanos.Reclutamientos
{
    public enum TipoArchivoEnum
    {
        [DescriptionAttribute("CV")]
        CV = 0,
        [DescriptionAttribute("SEGUIMIENTO FASES")]
        seguimientoFases = 1,
        [DescriptionAttribute("EXAMEN MEDICO")]
        examenMedico = 2,
        [DescriptionAttribute("ARCHIVO MAQUINARIA")]
        archivoMaquinaria = 3,
        [DescriptionAttribute("CONTRATO FIRMADO")]
        contratoFirmado = 4,
        [DescriptionAttribute("CV")]
        cv = 5,
        //5: CV
        [DescriptionAttribute("FOTO EMPLEADO")]
        fotoEmpleado = 6
    }
}
