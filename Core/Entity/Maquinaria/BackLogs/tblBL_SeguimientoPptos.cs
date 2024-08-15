using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.BackLogs
{
    public class tblBL_SeguimientoPptos
    {
        public int id { get; set; }
        public string areaCuenta { get; set; }
        public string folioPpto { get; set; }
        public int consecutivoFolio { get; set; }
        public DateTime fechaPpto { get; set; }
        public string noEconomico { get; set; }
        public decimal horas { get; set; }        
        public decimal Ppto { get; set; }
        public int idInspTMC { get; set; }
        public int esVobo1 { get; set; }
        public DateTime fechaVobo1 { get; set; }
        public string comentRechaVobo1 { get; set; }
        public int esVobo2 { get; set; }
        public DateTime fechaVobo2 { get; set; }
        public string comentRechaVobo2 { get; set; }
        public int esAutorizado { get; set; }
        public DateTime fechaAutorizado { get; set; }
        public string comentRechaAutorizado { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idCatMaquina { get; set; }

        public int EstatusSegPpto { get; set; }

        public int idFrente { get; set; }

        public virtual tblBL_CatFrentes lstCatFrentes { get; set; }
        public virtual tblM_CatMaquina lstCatMaquinas { get; set; }
        public virtual tblBL_InspeccionesTMC lstInspeccionesTMC { get; set; }

        public string firmaVobo1 { get; set; }
        public int idPuestoVobo1 { get; set; }
        public int idUserVobo1 { get; set; }
        public string firmaVobo2 { get; set; }
        public int idUserVobo2 { get; set; }
        public string firmaAutorizado { get; set; }
        public int idUserAutorizado { get; set; }

    }
}
