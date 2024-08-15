using Core.Entity.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class SegCandidatosDTO
    {
        public int id { get; set; }
        public string nombreCandidato { get; set; }
        public string correo { get; set; }
        public string celular { get; set; }
        public int idPuesto { get; set; }
        public string puesto { get; set; }
        public int idGestionSolicitud { get; set; }
        public bool puestoGeneral { get; set; }
        public int cantFases { get; set; }
        public List<int> lstFasesID { get; set; }
        public List<string> lstFasesStr { get; set; }
        public string nombre { get; set; }
        public string apePaterno { get; set; }
        public string apeMaterno { get; set; }
        public decimal progresoSeguimiento { get; set; }
        public bool seguimientoCandidatoActivo { get; set; }
        public bool esNecesarioAprobar { get; set; }
        public bool seguimientoCancelado { get; set; }
        public string esSeguimientoCancelado { get; set; }
        public int edad { get; set; }
        public string telefono { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public int? estatus { get; set; }
        public List<string> lstFiltroCC { get; set; }

        #region SEGUIMIENTO CANDIDATOS CREAR/EDITAR
        public int idCandidato { get; set; }
        public int idFase { get; set; }
        public int idSeg { get; set; }
        public int idActividad { get; set; }
        public decimal calificacion { get; set; }
        public int esAprobada { get; set; }
        public bool esOmitida { get; set; }
        public string comentario { get; set; }
        public DateTime fechaActividad { get; set; }
        public int? clave_empleado { get; set; }
        public bool? esReingreso { get; set; }
        #endregion
    }
}