using Core.DTO.RecursosHumanos.Prestamos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Constancias
{
    public class repPrestamosDTO
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int puesto { get; set; }
        public int clave_empleado { get; set; }
        public string ccDescripcion { get; set; }
        public int FK_Prestamo { get; set; }
        public bool esAutorizar { get; set; }
        public string nombrePuesto { get; set; }
        public string nombreCompleto { get; set; }
        public bool notificadoParaGestion { get; set; }
        public string empresa { get; set; }
        public string fecha_alta { get; set; }
        public string tipoNomina { get; set; }   
        public decimal sueldo_base { get; set; }    
        public decimal complemento { get; set; }    
        public decimal totalN { get; set; }       
        public decimal totalM { get; set; }
        public decimal otrosDescuento { get; set; }
        public decimal cantidadMax { get; set; }
        public decimal cantidadSoli { get; set; }
        public string cantidadLetra { get; set; }
        public decimal cantidadDescontar { get; set; }
        public string formaPago { get; set; }      
        public int motivoPrestamo { get; set; }
        public string justificacion { get; set; }
        public string tipoPuesto { get; set; }
        public string tipoPrestamo { get; set; }
        public string tipoSolicitud { get; set; }
        public string nombreResponsableCC { get; set; }        
        public string nombreDirectorLineaN { get; set; }
        public string nombreGerenteOdirector { get; set; }
        public string nombreDirectorGeneral { get; set; }
        public string nombreJefeInmediato { get; set; }
        public string nombreCapitalHumano { get; set; }
        public string estatus { get; set; }
        public int? idResponsableCC { get; set; }
        public int? idDirectorLineaN { get; set; }
        public int? idGerenteOdirector { get; set; }
        public int? idDirectorGeneral { get; set; }
        public DateTime fecha_creacion { get; set; }
        public int idCapitalHumano { get; set; }
        public string nombre { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public string DescripcionPuesto { get; set; }
        public string descripcionTipoPrestamo { get; set; }
        public bool puedeAutorizarRechazar { get; set; }
        public decimal bono_zona { get; set; }
        public int idUsuaioCreacion { get; set; }
        public string nombreUsuarioCreacion { get; set; }
        public DateTime fecha_altaEmpleado { get; set; }
        public int FK_Sindicato { get; set; } //VIEJO
        public string sindicato { get; set; }
        public decimal montoDescontados { get; set; } //
        public DateTime? fechaModificacion { get; set; }
        public string descCategoriaPuesto { get; set; }
        public string comentarioRechazo { get; set; }
        public List<AutorizantesPerstamosDTO> lstAuth { get; set; }
        public bool esSindicalizado { get; set; }
        public int cantidadArchivos { get; set; }
        public int consecutivo { get; set; }
    }
}