using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_Solicitudes
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int idPuesto { get; set; }
        public string puesto { get; set; }
        public int clave_depto { get; set; }
        public bool esGeneral { get; set; }
        public int idMotivo { get; set; }
        [ForeignKey("idMotivo")]
        public virtual tblRH_REC_CatMotivos virtualLstMotivos { get; set; }
        public string sexo { get; set; }
        public int rangoInicioEdad { get; set; }
        public int rangoFinEdad { get; set; }
        public int idEscolaridad { get; set; }
        [ForeignKey("idEscolaridad")]
        public virtual tblRH_REC_CatEscolaridades virtualLstEscolaridades { get; set; }
        public int clave_pais_nac { get; set; }
        public int clave_estado_nac { get; set; }
        public int clave_ciudad_nac { get; set; }
        public int aniosExp { get; set; }
        public string conocimientoGen { get; set; }
        public string expEspecializada { get; set; }
        public int cantVacantes { get; set; }
        public int cantVacantesCubiertas { get; set; }
        public bool esPuestoNuevo { get; set; }
        public bool terminado { get; set; }
        public DateTime? fechaAltaUltimaVacante { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
