using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class CandidatosDTO
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
        public string puestoDesc { get; set; }
        public int idPuesto { get; set; }        
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
        public string nombreCompleto { get; set; }
        public int idEstatus { get; set; }
        public string estatus { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public decimal altura { get; set; }
        public decimal peso { get; set; }
        public string notasReclutador { get; set; }
        public string sexo { get; set; }
        public int edad { get; set; }
        public string cc { get; set; }
        public int  puesto { get; set; }
        public bool tieneEntrevista { get; set; }
        public bool tieneArchivo { get; set; }
        public int? clave_empleado { get; set; }
        public bool? esReingreso { get; set; }
        public decimal calificacion { get; set; }
        public string comentario { get; set; }
        public string cuspp { get; set; }
        public int? PERU_departamento { get; set; }
        public string PERU_descDepartamento { get; set; }
    }
}
