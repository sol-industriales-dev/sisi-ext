using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Comercializacion.CRM
{
    public class tblAF_CRM_Proyectos
    {
        #region SQL
        public int id { get; set; }
        public string nombreProyecto { get; set; }
        public int FK_Cliente { get; set; }
        public int FK_Prioridad { get; set; }
        public int FK_Division { get; set; }
        public int FK_Municipio { get; set; }
        public decimal importeCotizadoAprox { get; set; }
        public DateTime fechaInicio { get; set; }
        public int FK_Estatus { get; set; }
        public int FK_Escenario { get; set; }
        public int FK_UsuarioResponsable { get; set; }
        public int FK_Riesgo { get; set; }
        public string descripcionObra { get; set; }
        public bool esProspecto { get; set; }
        public int FK_Canal { get; set; }
        public int FK_EstatusHistorial { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}