using Core.Entity.Administrativo.Contratistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.Indicadores
{
    public class IncidentesAgrupacionesContratistasDTO
    {
        public int id { get; set; }
        public string nomAgrupacion { get; set; }
        public int idAgruContratista { get; set; }
        public int idContratista { get; set; }
        List<int> lstContratistasID { get; set; }
        List<string> lstContratistas { get; set; }
        public bool esActivo { get; set; }
        public string nomContratista { get; set; }
    }
}
