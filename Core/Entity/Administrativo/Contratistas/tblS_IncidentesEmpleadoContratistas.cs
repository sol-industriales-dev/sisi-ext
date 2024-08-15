using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contratistas
{
    public class tblS_IncidentesEmpleadoContratistas
    {
        public int id { get; set; }
        public int idEmpresaContratista { get; set; }
        public string nombre { get; set; }
        public string apePaterno { get; set; }
        public string apeMaterno { get; set; }
        public string domicilio { get; set; }
        public string colonia { get; set; }
        public int idPais { get; set; }
        public int idEstado { get; set; }
        public int idCiudad { get; set; }
        public string codigoPostal { get; set; }
        public string UMF { get; set; }
        public string sexo { get; set; }
        public string localidadNacimiento { get; set; }
        public string estadoNacimiento { get; set; }
        public string numeroSeguroSocial { get; set; }
        public string numeroDeIdentificacionOficial { get; set; }
        public string rfc { get; set; }
        public string curp { get; set; }
        public string nombrePadre { get; set; }
        public string nombreMadre { get; set; }
        public string nombreEspo { get; set; }
        public string beneficiario { get; set; }
        public string parentesco { get; set; }
        public string calzado { get; set; }
        public string tipoSangre { get; set; }
        public string estadoCivil { get; set; }
        public string tallaCamisa { get; set; }
        public string alergias { get; set; }
        public string tipoVivienda { get; set; }
        public string tallaPantalon { get; set; }
        public string overoll { get; set; }
        public string hijos { get; set; }
        public string edades { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string puesto { get; set; }
        public string centroDeCostos { get; set; }
        public string jefeInmediato { get; set; }
        public decimal sueldoBase { get; set; }
        public decimal complento { get; set; }
        public decimal total { get; set; }
        public bool esActivo { get; set; }

    }
}
