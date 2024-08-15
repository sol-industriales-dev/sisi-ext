using Core.Entity.Administrativo.DocumentosXPagar.Especiales;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar.Especiales
{
    public class tblAF_DxP_CCContratoMapping : EntityTypeConfiguration<tblAF_DxP_CCContrato>
    {
        public tblAF_DxP_CCContratoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblAF_DxP_CCContrato");
        }
    }
}
