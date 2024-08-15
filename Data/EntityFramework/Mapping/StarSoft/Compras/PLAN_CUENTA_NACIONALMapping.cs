using Core.Entity.StarSoft.OrdenCompra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.EntityFramework.Mapping.StarSoft.Compras
{
    public class PLAN_CUENTA_NACIONALMapping : EntityTypeConfiguration<PLAN_CUENTA_NACIONAL>
    {
        public PLAN_CUENTA_NACIONALMapping() 
        {
            HasKey(x => x.PLANCTA_CODIGO);
            Property(x => x.PLANCTA_CODIGO).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("PLANCTA_CODIGO");
            ToTable("PLAN_CUENTA_NACIONAL");
        }
    }
}