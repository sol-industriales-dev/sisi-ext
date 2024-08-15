using Core.Entity.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class GestionCandidatosDTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apePaterno { get; set; }
        public string apeMaterno { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string nss { get; set; }
        public int pais { get; set; }
        public string paisDesc { get; set; }
        public int estado { get; set; }
        public string estadoDesc { get; set; }
        public int municipio { get; set; }
        public string municipioDesc { get; set; }
        public int idGestionSolicitud { get; set; }
        //public virtual tblRH_REC_GestionSolicitudes virtualLstGestionSolicitudes { get; set; }
        [ForeignKey("idGestionSolicitud")]  
        public virtual tblRH_REC_Solicitudes virtualLstSolicitudes { get; set; }

        public int estatus { get; set; }
        public decimal altura { get; set; }
        public decimal peso { get; set; }
        public string notasReclutador { get; set; }
        public string sexo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string RFC { get; set; }
        public int idPuesto { get; set; }
        public string puestoDesc { get; set; }
        public string CURP { get; set; }
        public string estadoCivil { get; set; }
        public string escolaridad { get; set; }
        public List<DocumentoMedicoDTO> documentosMedicos { get; set; }

        public GestionCandidatosDTO()
        {
            documentosMedicos = new List<DocumentoMedicoDTO>();
        }

        public int idDepartamento { get; set; }
        public string departamentoDesc { get; set; }
        public string ccSolicitud { get; set; }

        //esDIANA var para habilitar el campo de fecha de ingreso solo si es diana alvarez
        public bool? esDiana { get; set; }

        //GESTION CANDIDATOS REINGRESO
        public int? clave_empleado { get; set; }
        public bool esCandiReingreso { get; set; }
        public string cuspp { get; set; }
        public int? PERU_departamento { get; set; }
        public string PERU_descDepartamento { get; set; }
    }
}
