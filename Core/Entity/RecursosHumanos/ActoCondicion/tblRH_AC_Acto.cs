using Core.Entity.Principal.Usuarios;
using Core.Enum.RecursosHumanos.ActoCondicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.ActoCondicion
{
    public class tblRH_AC_Acto
    {
        public int id { get; set; }
        public int claveEmpleado { get; set; }
        public string nombre { get; set; }
        public string puesto { get; set; }
        public DateTime fechaIngreso { get; set; }
        public int accionID { get; set; }
        public TipoActoCH tipoActo { get; set; }
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
        public virtual tblRH_AC_Departamentos departamento { get; set; }
        public int subclasificacionDepID { get; set; }
        public EstatusActoCondicionCH estatus { get; set; }
        public DateTime? fechaProcesoCompleto { get; set; }
        public int? numeroInfraccion { get; set; }
        public int? nivelInfraccion { get; set; }
        public int? nivelInfraccionAcumulado { get; set; }
        public int? numeroFalta { get; set; }
        public string compromiso { get; set; }
        public string rutaEvidencia { get; set; }
        public bool activo { get; set; }
        public int usuarioCreadorID { get; set; }

        public virtual tblRH_AC_Accion accion { get; set; }
        public virtual tblRH_AC_Clasificacion clasificacion { get; set; }
        //public virtual tblRH_AC_IncidentesTipoProcedimientosViolados procedimientoViolado { get; set; }
        public virtual tblP_Usuario usuarioCreador { get; set; }
        public int idEmpresa { get; set; }
        public int? idAgrupacion { get; set; }
        //public virtual tblRH_AC_IncidentesAgrupacionCC agrupacion { get; set; }

        public string firmaEmpleado { get; set; }
        public string firmaSupervisor { get; set; }
        public string firmaSST { get; set; }
        public int? claveSST { get; set; }
        public string nombreSST { get; set; }
        public DateTime? fechaFirmado { get; set; }
        public string rutaActa { get; set; }
        public DateTime? fechaCargaActa { get; set; }

        public int? clasificacionGeneralID { get; set; }
        public virtual tblRH_AC_ClasificacionGeneral clasificacionGeneral { get; set; }

        public bool? cargaMasiva { get; set; }
        public string hallazgo { get; set; }
    }
}
