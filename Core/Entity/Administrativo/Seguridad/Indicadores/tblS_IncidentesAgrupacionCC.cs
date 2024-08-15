using Core.Enum.Administracion.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesAgrupacionCC
    {
        public int id { get; set; }
        public string nomAgrupacion { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public string codigo { get; set; }
    }
}
