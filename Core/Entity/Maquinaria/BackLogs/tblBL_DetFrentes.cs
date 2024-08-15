using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Maquinaria.BackLogs
{
    public class tblBL_DetFrentes
    {
        public int id { get; set; }
        public int idFrente{ get; set; }
        public int idSeguimientoPpto { get; set; }
        public DateTime fechaAsignacion { get; set; }
        public decimal avance { get; set; }
        public int idInspTMC { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }

        //TABLAS VIRTUALES
        public virtual tblBL_CatFrentes lstCatFrentes { get; set; }
        public virtual tblBL_SeguimientoPptos lstSeguimiento { get; set; }
        public virtual tblBL_InspeccionesTMC lstInspeccionesTMC { get; set; }
    }
}
