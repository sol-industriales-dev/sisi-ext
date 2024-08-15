using Core.Entity.StarSoft.OrdenCompra;
using Core.Entity.StarSoft.Requisiciones;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.StarSoft;

namespace Data.EntityFramework.Context
{
    public class MainContextPeruStarSoft003BDCONTABILIDAD : DbContext
    {
        public DbSet<BANCO> BANCO { get; set; }
        public DbSet<CUENTA_TELE_ANEXO> CUENTA_TELE_ANEXO { get; set; }
        public DbSet<TIPO_CAMBIO> TIPO_CAMBIO { get; set; }
        public DbSet<PLAN_CUENTA_NACIONAL> PLAN_CUENTA_NACIONAL { get; set; }
        public DbSet<ANEXO> ANEXO { get; set; }

        public MainContextPeruStarSoft003BDCONTABILIDAD()
            : base("MainContextPeruStarSoft003BDCONTABILIDAD")
        {
            //Disable initializer
            Database.SetInitializer<MainContextPeruStarSoft003BDCONTABILIDAD>(null);
        }
    }

}
