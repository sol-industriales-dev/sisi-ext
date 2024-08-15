using Core.Enum.Administracion.Seguridad.ActoCondicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.Administracion.Seguridad.ActoCondicion
{
    public class ActoDTO
    {
        public int id { get; set; }
        public int folio { get; set; }
        public int claveEmpleado { get; set; }
        public string nombre { get; set; }
        public string puesto { get; set; }
        public string fechaIngreso { get; set; }
        public int accionID { get; set; }
        public TipoActo tipoActo { get; set; }
        public TipoRiesgo tipoRiesgo { get; set; }
        public bool esExterno { get; set; }
        public int? claveContratista { get; set; }
        public int claveInformo { get; set; }
        public string nombreInformo { get; set; }
        public string fechaCreacion { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public int clasificacionID { get; set; }
        public int procedimientoID { get; set; }
        public string fechaSuceso { get; set; }
        public int claveSupervisor { get; set; }
        public string nombreSupervisor { get; set; }
        public int departamentoID { get; set; }
        public int subclasificacionDepID { get; set; }
        public int estatus { get; set; }
        public HttpPostedFileBase archivoEvidencia { get; set; }
        public int idEmpresa { get; set; }
        public int idAgrupacion { get; set; }
        public int numeroInfraccion { get; set; }
        public string descripcionInfraccion { get; set; }
        public int nivelInfraccion { get; set; }
        public int nivelInfraccionAcumulado { get; set; }
        public int numeroFalta { get; set; }
        public string compromiso { get; set; }
        public List<CausaAccionDTO> causas { get; set; }
        public List<CausaAccionDTO> acciones { get; set; }
        public bool tieneEvidencia { get; set; }
        public bool firmadoPorEmpleado { get; set; }
        public bool firmadoPorSupervisor { get; set; }
        public bool firmadoPorSST { get; set; }
        public int? clasificacionGeneralID { get; set; }
    }
}
