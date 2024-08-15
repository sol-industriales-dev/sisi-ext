using Core.Entity.Administrativo.Seguridad.Indicadores;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Administracion.Seguridad.ActoCondicion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.ActoCondicion
{
    public class tblSAC_Acto
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string nombre { get; set; }
        public string puesto { get; set; }
        public DateTime fechaIngreso { get; set; }
        public int accionID { get; set; }
        public TipoActo tipoActo { get; set; }
        public bool esExterno { get; set; }
        public int? claveContratista { get; set; }
        public int claveInformo { get; set; }
        public string nombreInformo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string cc { get; set; }
        public int folio { get; set; }
        public string descripcion { get; set; }
        public int clasificacionID { get; set; }
        public int procedimientoID { get; set; }
        public DateTime fechaSuceso { get; set; }
        public int claveSupervisor { get; set; }
        public string nombreSupervisor { get; set; }
        public int departamentoID { get; set; }
        public virtual tblSAC_Departamentos departamento { get; set; }
        public int subclasificacionDepID { get; set; }
        public EstatusActoCondicion estatus { get; set; }
        public DateTime? fechaProcesoCompleto { get; set; }
        public int? numeroInfraccion { get; set; }
        public int? nivelInfraccion { get; set; }
        public int? nivelInfraccionAcumulado { get; set; }
        public int? numeroFalta { get; set; }
        public string compromiso { get; set; }
        public string rutaEvidencia { get; set; }
        public bool activo { get; set; }
        public int usuarioCreadorID { get; set; }

        public virtual tblSAC_Accion accion { get; set; }
        public virtual tblSAC_Clasificacion clasificacion { get; set; }
        public virtual tblS_IncidentesTipoProcedimientosViolados procedimientoViolado { get; set; }
        public virtual tblP_Usuario usuarioCreador { get; set; }
        public int idEmpresa { get; set; }
        public int? idAgrupacion { get; set; }
        public virtual tblS_IncidentesAgrupacionCC agrupacion { get; set; }

        public string firmaEmpleado { get; set; }
        public string firmaSupervisor { get; set; }
        public string firmaSST { get; set; }
        public int? claveSST { get; set; }
        public string nombreSST { get; set; }
        public DateTime? fechaFirmado { get; set; }
        public string rutaActa { get; set; }
        public DateTime? fechaCargaActa { get; set; }

        public int? clasificacionGeneralID { get; set; }
        public virtual tblSAC_ClasificacionGeneral clasificacionGeneral { get; set; }

        public bool? cargaMasiva { get; set; }
    }
}
