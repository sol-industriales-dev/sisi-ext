using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_STB_EconomicoBloqueado
    {
        public int id { get; set; }
        public string noEconomico { get; set; }
        public string ccEconomico { get; set; }
        public bool registroActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int usuarioRegistro { get; set; }
        public int usuarioModifico { get; set; }
    }
}