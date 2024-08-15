using Core.Entity.StarSoft.PlantillasCatalogos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.EntityFramework.Context
{
    public class MainContextPeruStarSoftSTARPLAN : DbContext
    {
        #region TABLAS
        public DbSet<RTPS_REGIMEN_LABORAL> RTPS_REGIMEN_LABORAL { get; set; }
        public DbSet<RTPS_REGIMEN_ESSALUD> RTPS_REGIMEN_ESSALUD { get; set; }

        #endregion

        public MainContextPeruStarSoftSTARPLAN()
            : base("MainContextPeruStarSoftSTARPLAN")
        {
            //Disable initializer
            Database.SetInitializer<MainContextPeruStarSoftSTARPLAN>(null);
        }
    }
}
