using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Mantenimiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Mantenimiento2
{
    public class tblM_PMComponenteModelo
    {
        public int id { get; set; }

        public int componenteID { get; set; }
        public int modeloID { get; set; }
        public bool estatus { get; set; }
        public int usuariosCaptura { get; set; }
        public DateTime fechaCaptura { get; set; }

        public virtual tblM_CatModeloEquipo Modelo { get; set; }
        public virtual tblM_CatComponentesViscosidades Componente { get; set; }
    }
}
