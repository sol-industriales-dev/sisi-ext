using Core.Entity.Administrativo.DocumentosXPagar.PQ;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar.PQ
{
    public class tblAF_DxP_PQ_AbonoMapping : EntityTypeConfiguration<tblAF_DxP_PQ_Abono>
    {
        public tblAF_DxP_PQ_AbonoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            HasRequired(x => x.pq).WithMany().HasForeignKey(x => x.pqID);
            HasRequired(x => x.usuario).WithMany().HasForeignKey(x => x.usuarioCreacionID);
            ToTable("tblAF_DxP_PQ_Abono");
        }
    }
}
