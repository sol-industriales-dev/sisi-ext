using Core.Entity.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Portal
{

    public class RequirementPurchaseOrderMapping : EntityTypeConfiguration<RequirementPurchaseOrder>
    {
        public RequirementPurchaseOrderMapping()
        {
            HasKey(x => x.ID);
            Property(x => x.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("RequirementPurchaseOrder");
        }
    }
}
