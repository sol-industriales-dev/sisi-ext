using Core.Entity.StarSoft.Plantillas;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Context
{
    public class MainContextPeruStarSoftPLCONSTRU : DbContext
    {
        #region TABLAS
        public DbSet<AFPS> AFPS { get; set; }
        public DbSet<TIPOSTRAB> TIPOSTRAB { get; set; }
        public DbSet<SITUACION> SITUACION { get; set; }
        public DbSet<EPS> EPS { get; set; }
        public DbSet<CENTROSAR> CENTROSAR { get; set; }

        #endregion

        public MainContextPeruStarSoftPLCONSTRU()
            : base("MainContextPeruStarSoftPLCONSTRU")
        {
            //Disable initializer
            Database.SetInitializer<MainContextPeruStarSoftPLCONSTRU>(null);
        }
    }
}
