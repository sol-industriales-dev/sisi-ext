using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class EntrevistaInicialDTO
    {
        public int id { get; set; }
        public int idCandidato { get; set; }
        public int idEscolaridad { get; set; }
        public string estadoCivil { get; set; }
        public string lugarNacimiento { get; set; }
        public string expectativaSalarial { get; set; }
        public bool? tipoSalario { get; set; } // true: BRUTO false: NETO
        public string puestoSolicitado { get; set; }
        public string puestoSolicitadoDesc { get; set; }
        public string expLaboral { get; set; }
        public string sectorCiudad { get; set; }
        public string tiempoEnLaCiudad { get; set; }
        public bool entrevistasAnteriores { get; set; }
        public int idPlataforma { get; set; }
        public string documentacion { get; set; }
        public bool familiarEnEmpresa { get; set; }
        public string telefono { get; set; }
        public string familia { get; set; }
        public string empleos { get; set; }
        public string caracteristicasCandidato { get; set; }
        public string comentarioEntrevistador { get; set; }
        public DateTime fechaEntrevista { get; set; }
        //public bool disposicionHorario { get; set; }
        public string disposicionHorario { get; set; }
        public bool avanza { get; set; }
        public string comentariosAvanza { get; set; }
        public int idUsuarioEntrevisto { get; set; }
        public string resultado { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public int idRegistrado { get; set; }
        public string paisDesc { get; set; }
        public string estadoDesc { get; set; }
        public string municipioDesc { get; set; }
        public string familiaEnLaEmpresa { get; set; }

        #region CAMPOS SEGUIMIENTO
        public string nombre { get; set; }
        public string apePaterno { get; set; }
        public string apeMaterno { get; set; }
        public int edad { get; set; }
        public DateTime fechaNacimiento { get; set; }
        #endregion
    }
}