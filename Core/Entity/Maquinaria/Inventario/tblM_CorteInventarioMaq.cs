using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_CorteInventarioMaq
    {
        public int Id { get; set; }
        public DateTime FechaCorte { get; set; }
        public int IdUsuarioCorte { get; set; }
        public int? IdTipoMaquina { get; set; }
        public bool Estatus { get; set; }
        public bool Bloqueado { get; set; }
        public bool BloqueadoConstruplan { get; set; }
        public DateTime FechaCreacion { get; set; }

        [ForeignKey("IdUsuarioCorte")]
        public virtual tblP_Usuario Usuario { get; set; }

        [ForeignKey("IdTipoMaquina")]
        public virtual tblM_CatTipoMaquinaria TipoMaquina { get; set; }

        [ForeignKey("IdCorteInvMaq")]
        public virtual List<tblM_CorteInventarioMaq_Detalle> DetalleInv { get; set; }
    }
}