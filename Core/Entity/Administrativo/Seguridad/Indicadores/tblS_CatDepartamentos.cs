using Core.Entity.Administrativo.Seguridad.CatDepartamentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_CatDepartamentos
    {
        public int id { get; set; }
        public int clave_depto { get; set; }
        //public virtual tblM_CatMaquina catMaquina { get; set; }
        public string descripcion { get; set; }
        public string cc { get; set; }
        public int idAreaOperativa { get; set; }
        //public virtual tblS_CatAreasOperativas catAreasOperativas { get; set; }
        public bool esActivo { get; set; }
        public int idEmpresa { get; set; }
    }
}
