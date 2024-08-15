using Core.Enum.Administracion.Seguridad.Indicadores;
using System;
using System.Collections.Generic;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesAgrupacionCCDet
    {
        public int id { get; set; }
        public int idAgrupacionCC { get; set; }
        public virtual tblS_IncidentesAgrupacionCC idAgrupacion { get; set; }
        public string cc { get; set; }
        public int idEmpresa { get; set; }
        public bool esActivo { get; set; }

    }
}
