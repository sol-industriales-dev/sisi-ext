using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Indicadores
{
    public enum tipoAccientabilidadEnum
    {
        [DescriptionAttribute("Proyecto")]
        Proyecto = 1,
        [DescriptionAttribute("Puesto")]
        Puesto = 2,
        [DescriptionAttribute("Departamento")]
        Departamento = 3,
        [DescriptionAttribute("Severidad")]
        Severidad = 4,
        [DescriptionAttribute("Dia")]
        Dia = 5,
        [DescriptionAttribute("Hora")]
        Hora = 6,
        [DescriptionAttribute("Turno")]
        Turno = 7,
        [DescriptionAttribute("Actividad")]
        Actividad = 8,
        [DescriptionAttribute("Tarea")]
        Tarea = 9,
        [DescriptionAttribute("Agente")]
        Agente = 10,
        [DescriptionAttribute("Edad")]
        Edad = 11,
        [DescriptionAttribute("Experiencia")]
        Experiencia = 12,
        [DescriptionAttribute("Antigüedad")]
        Antigüedad = 13,
        [DescriptionAttribute("DiasTrabajados")]
        DiasTrabajados = 14,
        [DescriptionAttribute("Lugar")]
        Lugar = 15,
        [DescriptionAttribute("Capacitado")]
        Capacitado = 16,
        [DescriptionAttribute("ProtocoloTrabajo")]
        ProtocoloTrabajo = 17,
        [DescriptionAttribute("TipoContacto")]
        TipoContacto = 18,
    }
}
