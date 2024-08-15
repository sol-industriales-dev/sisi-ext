using Core.Entity.StarSoft;
using Core.Entity.StarSoft.Almacen;
using Core.Entity.StarSoft.OrdenCompra;
using Core.Entity.StarSoft.Requisiciones;
using Data.EntityFramework.Mapping.StarSoft;
using Data.EntityFramework.Mapping.StarSoft.Requisiciones;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Context
{
    public class MainContextPeruStarSoft003BDCOMUN : DbContext
    {
        #region GENERALES

        public DbSet<TIPO_ARTICULO> TIPO_ARTICULO { get; set; }
        public DbSet<FAMILIA> FAMILIA { get; set; }
        public DbSet<MAECLI> MAECLI { get; set; }
        public DbSet<TABUNIMED> TABUNIMED { get; set; }
        #endregion

        #region REQUISICIONES

        public DbSet<REQUISC> REQUISC { get; set; }
        public DbSet<REQUISD> REQUISD { get; set; }
        public DbSet<MAEART> MAEART { get; set; }
        public DbSet<NUM_DOCCOMPRAS> NUM_DOCCOMPRAS { get; set; }
        public DbSet<TABAYU> TABAYU { get; set; }
        public DbSet<AREA> AREA { get; set; }
        public DbSet<TABALM> TABALM { get; set; }
        public DbSet<MAEPROV> MAEPROV { get; set; }
        #endregion

        #region ORDEN COMPRA
        public DbSet<COMOVC> COMOVC { get; set; }
        public DbSet<COMOVD> COMOVD { get; set; }
        public DbSet<COMOVC_S> COMOVC_S { get; set; }
        public DbSet<COMOVD_S> COMOVD_S { get; set; }
        public DbSet<RESPONSABLECMP> RESPONSABLECMP { get; set; }

        #endregion

        #region PROVEEDORES
        public DbSet<CUENTA_CORRIENTE_PROV> CUENTA_CORRIENTE_PROV { get; set; }
        #endregion
        #region ALMACEN
        public DbSet<MOVALMCAB> MOVALMCAB { get; set; }
        public DbSet<MovAlmDet> MovAlmDet { get; set; }
        public DbSet<STKART> STKART { get; set; }
        public DbSet<MoResMes> MoResMes { get; set; }
        public DbSet<KARDEX_VAL> KARDEX_VAL { get; set; }
        public DbSet<MOVINGCAB_S> MOVINGCAB_S { get; set; }
        public DbSet<MOVINGDET_S> MOVINGDET_S { get; set; }
        public DbSet<COMGUICAB> COMGUICAB { get; set; }
        public DbSet<COMGUIDET> COMGUIDET { get; set; }
        #endregion
        public DbSet<FORMA_PAGO> FORMA_PAGO { get; set; }
        public DbSet<TIPO_DOCU> TIPO_DOCU { get; set; }

        public MainContextPeruStarSoft003BDCOMUN()
            : base("MainContextPeruStarSoft003BDCOMUN")
        {
            //Disable initializer
            Database.SetInitializer<MainContextPeruStarSoft003BDCOMUN>(null);
        }
    }
}
