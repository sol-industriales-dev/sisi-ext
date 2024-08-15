using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class tendenciasAccidentabilidadDTO
    {
        public string dia { get; set; }
        public decimal porcentajeDia { get; set; }
        public string hora { get; set; }
        public decimal porcentajeHora { get; set; }
        public string turno { get; set; }
        public decimal porcentajeTurno { get; set; }
        public string cc { get; set; }
        public decimal porcentajeCC { get; set; }
        public string actividad { get; set; }
        public decimal porcentajeActividad { get; set; }
        public string tarea { get; set; }
        public decimal porcentajeTarea { get; set; }
        public string agente { get; set; }
        public decimal porcentajeAgente { get; set; }
        public string edad { get; set; }
        public decimal porcentajeEdad { get; set; }
        public string puesto { get; set; }
        public decimal porcentajePuesto { get; set; }
        public string experiencia { get; set; }
        public decimal porcentajeExperiencia { get; set; }
        public string antiguedadEmpresa { get; set; }
        public decimal porcentajeAntiguedadEmpresa { get; set; }
        public string diasTrabajados { get; set; }
        public decimal porcentajeDiasTrabajados { get; set; }
        public string departamento { get; set; }
        public decimal porcentajeDepartamento { get; set; }
        public string lugar { get; set; }
        public decimal porcentajeLugar { get; set; }
        public string tipoContacto { get; set; }
        public decimal porcentajeTipoContacto { get; set; }
        public string capacitado { get; set; }
        public decimal porcentajeCapacitado { get; set; }
        public string protocoloTrabajo { get; set; }
        public decimal porcentajeProtocolo { get; set; }
        public string potencialSeveridad { get; set; }
        public decimal porcentajePotencial { get; set; }
    }
}
