using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class informacionColaboradorDTO
    {
        public int id { get; set; }
        public decimal horasHombre { get; set; }
        public int lostDay { get; set; }
        public string cc { get; set; }
        public string fechaInicioStr { get; set; }
        public string fechaFinStr { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public bool puedeSerEliminado { get; set; }
        public int idEmpresa { get; set; }
        public int idAgrupacion { get; set; }
        public tblS_IncidentesAgrupacionCC agrupacion { get; set; }
    }
}
