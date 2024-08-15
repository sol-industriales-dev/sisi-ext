using Core.Enum.RecursosHumanos.ActoCondicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.RecursosHumanos.ActoCondicion
{
    public class CondicionDTO
    {
        public int id { get; set; }
        public int folio { get; set; }
        public HttpPostedFileBase imagenAntes { get; set; }
        public HttpPostedFileBase imagenDespues { get; set; }
        public string fechaResolucion { get; set; }
        public bool tieneImagenDespues { get; set; }
        public int claveInformo { get; set; }
        public string nombreInformo { get; set; }
        public string fechaCreacion { get; set; }
        public string cc { get; set; }
        public TipoRiesgoCH tipoRiesgo { get; set; }
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
        public string accionCorrectiva { get; set; }
        public int nivelPrioridad { get; set; }
        public bool tieneEvidencia { get; set; }
        public bool firmadoPorSupervisor { get; set; }
        public bool firmadoPorSST { get; set; }
        public int? clasificacionGeneralID { get; set; }
    }
}
