using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Reportes
{
    public class tblRH_EK_Prestamos
    {

        public int id { get; set; }
        public string cc { get; set; }
        public int puesto { get; set; }
        public int clave_empleado { get; set; }

        //public string ccDescripcion { get; set; }
        //public string nombrePuesto { get; set; }
        //public string nombreCompleto { get; set; }
        public string tipoNomina { get; set; }
        public string fecha_alta { get; set; }
        public decimal sueldo_base { get; set; }
        public decimal complemento { get; set; }
        public decimal totalN { get; set; }
        public decimal totalM { get; set; }
        public decimal otrosDescuento { get; set; }
        public decimal cantidadMax { get; set; }
        public decimal cantidadSoli { get; set; }       
        public decimal cantidadDescontar { get; set; }
        public string formaPago { get; set; }       
        public int motivoPrestamo { get; set; }
        public string justificacion { get; set; }
        public string estatus { get; set; }
        public string cantidadLetra { get; set; }
        public string tipoSolicitud { get; set; }
        public string tipoPuesto { get; set; }
        public string tipoPrestamo { get; set; }
        public string empresa { get; set; }
        public bool notificadoParaGestion { get; set; }
        public string comentarioRechazo { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuaioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public int idResponsableCC { get; set; }
        public int idDirectorLineaN { get; set; }
        public int idGerenteOdirector { get; set; }
        public int idDirectorGeneral { get; set; }
        public int idCapitalHumano { get; set; }
        public int consecutivo { get; set; }
        public bool registroActivo { get; set; }
    }
}
