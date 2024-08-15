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
    public class tblAF_DxP_RelInstitucionCtaMapping : EntityTypeConfiguration<tblAF_DxP_RelInstitucionCta>
    {
        public tblAF_DxP_RelInstitucionCtaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.complementaria).HasColumnName("complementaria");
            Property(x => x.cta).HasColumnName("cta");
            Property(x => x.digito).HasColumnName("digito");
            Property(x => x.institucionID).HasColumnName("institucionID");
            Property(x => x.moneda).HasColumnName("moneda");
            Property(x => x.scta).HasColumnName("scta");
            Property(x => x.sscta).HasColumnName("sscta");


            ToTable("tblAF_DxP_RelInstitucionCta");
        }
    }
}
