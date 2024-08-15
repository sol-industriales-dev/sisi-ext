using Core.Entity.Administrativo.Seguridad.ActoCondicion;
using Core.Entity.Administrativo.Seguridad.Indicadores;
using Core.Enum.Administracion.Seguridad.ActoCondicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.ActoCondicion
{
    public class ActoCondicionDTO
    {
        public int id { get; set; }
        public int folio { get; set; }
        public string cc { get; set; }
        public string proyecto { get; set; }
        public TipoRiesgo tipoRiesgo { get; set; }
        public string tipoRiesgoDesc { get; set; }
        public EstatusActoCondicion estatus { get; set; }
        public string estatusDesc { get; set; }
        public string fechaSuceso { get; set; }
        public bool puedeEliminar { get; set; }
        public int idAgrupacion { get; set; }
        public int idEmpresa { get; set; }
        public bool firmado { get; set; }
        public int? nivelPrioridad { get; set; }
        public DateTime fechaCondicion { get; set; }
        public tblSAC_ClasificacionPrioridad prioridad { get; set; }
        public string nomAgrupacion { get; set; }
        public tblS_IncidentesAgrupacionCC agrupacion { get; set; }
        public DateTime? fechaFirmado { get; set; }
        public DateTime fechaSucesoDT { get; set; }
        public TipoActo tipoActo { get; set; }
        public string subclasificacionDep { get; set; }
        public int subclasificacionDepID { get; set; }
        public int departamentoID { get; set; }
    }
}
