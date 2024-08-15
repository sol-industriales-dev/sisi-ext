using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.BackLogs
{
    public class tblBL_InspeccionesTMC
    {
        public int id { get; set; }
        public string areaCuenta { get; set; }
        public int periodo { get; set; }
        public string noEconomico { get; set; }
        public decimal horas { get; set; }
        public int idCatMaquina { get; set; }
        public bool esRehabilitar { get; set; }
        public int idMotivo { get; set; }
        public DateTime fechaRequerido { get; set; }
        public DateTime fechaPromesa { get; set; }
        public DateTime fechaCreacionInsp { get; set; }
        public DateTime fechaModificacionInsp { get; set; }
        public bool esActivo { get; set; }

        public virtual tblM_CatMaquina lstCatMaquinas { get; set; }
    
    }
}
