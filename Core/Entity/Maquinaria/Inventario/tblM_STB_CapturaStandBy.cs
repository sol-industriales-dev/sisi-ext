using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_STB_CapturaStandBy
    {
        public int id { get; set; }
        public string Economico { get; set; }
        public int noEconomicoID { get; set; }
        public virtual tblM_CatMaquina noEconomico { get; set; }
        public int estatus { get; set; }
        public int usuarioCapturaID { get; set; }
        public virtual tblP_Usuario usuarioCaptura { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int? usuarioAutorizaID { get; set; }
        public virtual tblP_Usuario usuarioAutoriza { get; set; }
        public DateTime? fechaAutoriza { get; set; }
        public int? usuarioLiberaID { get; set; }
        public virtual tblP_Usuario usuarioLibera { get; set; }
        public DateTime? fechaLibera { get; set; }
        public string ccActual { get; set; }
        public string comentarioJustificacion { get; set; }
        public string comentarioValidacion { get; set; }
        public string comentarioLiberacion { get; set; }
        public string evidenciaJustificacion { get; set; }

        public decimal moiEquipo { get; set; }
        public decimal valorEnLibroEquipo { get; set; }
        public decimal depreciacionMensualEquipo { get; set; }

        public decimal valorEnLibroOverhaul { get; set; }
        public decimal depreciacionMensualOverhaul { get; set; }
        public bool esVoBo { get; set; }
        public int FK_UsuarioVoBo { get; set; }
        public DateTime? fechaVoBo { get; set; }
    }
}
