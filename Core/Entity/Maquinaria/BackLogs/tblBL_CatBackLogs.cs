using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.BackLogs
{
    public class tblBL_CatBackLogs
    {
        public int id { get; set; }
        public string folioBL { get; set; }
        public DateTime fechaInspeccion { get; set; }
        public string cc { get; set; }
        public string noEconomico { get; set; }
        public decimal horas { get; set; }
        public int idSubconjunto { get; set; }
        public virtual tblBL_CatSubconjuntos subconjunto { get; set; }
        public bool parte { get; set; }
        public bool manoObra { get; set; }
        public string descripcion { get; set; }
        public decimal totalMX { get; set; }
        public string areaCuenta { get; set; }
        public int idEstatus { get; set; }
        public decimal presupuestoEstimado { get; set; }
        public int tipoBL { get; set; }
        public int idSegPpto { get; set; }
        public bool esActivo { get; set; }
        public bool esLiberado { get; set; }
        public int idUsuarioResponsable { get; set; }
        public DateTime fechaCreacionBL { get; set; }
        public DateTime fechaModificacionBL { get; set; }
        public DateTime fechaLiberadoBL { get; set; }
        public DateTime? fechaInstaladoBL { get; set; }
        public int periodoRegistro { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
    }
}