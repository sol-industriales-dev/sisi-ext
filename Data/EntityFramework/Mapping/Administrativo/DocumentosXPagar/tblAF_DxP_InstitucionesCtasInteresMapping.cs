using Core.Entity.Administrativo.DocumentosXPagar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_InstitucionesCtasInteresMapping : EntityTypeConfiguration<tblAF_DxP_InstitucionesCtasInteres>
    {
        public tblAF_DxP_InstitucionesCtasInteresMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            ToTable("tblAF_DxP_InstitucionesCtasInteres"); ;
        }
    }
}
