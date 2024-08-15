using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class GestionSolicitudesDTO
    {
        public int id { get; set; }
        public int idSolicitud { get; set; }
        public string cc { get; set; }
        public string ccDescripcion { get; set; }
        public int idPuesto { get; set; }
        public string puesto { get; set; }
        public int idMotivo { get; set; }
        public string motivo { get; set; }
        public string sexo { get; set; }
        public int rangoInicioEdad { get; set; }
        public int rangoFinEdad { get; set; }
        public int idEscolaridad { get; set; }
        public string escolaridad { get; set; }
        public int clave_pais_nac { get; set; }
        public string pais { get; set; }
        public int clave_estado_nac { get; set; }
        public string estado { get; set; }
        public int clave_ciudad_nac { get; set; }
        public string ciudad { get; set; }
        public int aniosExp { get; set; }
        public string conocimientoGen { get; set; }
        public string expEspecializada { get; set; }
        public int cantVacantes { get; set; }
        public int cantVacantesCubiertas { get; set; }
        public bool esPuestoNuevo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
        public string motivoRechazo { get; set; }
        public bool esAutorizada { get; set; }
        public string strAutorizada { get; set; }
        public int personalExistente { get; set; }
        public int personalRequerido { get; set; }
        public int cantVacantesPendientes { get; set; }
    }
}
