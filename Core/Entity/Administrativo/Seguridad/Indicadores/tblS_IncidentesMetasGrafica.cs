using Core.Enum.Administracion.Seguridad.Indicadores;
using System;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesMetasGrafica
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public decimal valor { get; set; }
        public int año { get; set; }
        public string colorString { get; set; }
        public TipoGraficaEnum tipoGrafica { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCreadorID { get; set; }
    }
}
