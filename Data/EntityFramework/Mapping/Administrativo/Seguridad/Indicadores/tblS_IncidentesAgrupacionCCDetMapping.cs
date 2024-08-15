using Core.Entity.Administrativo.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.AgrupacionCC
{
    public class tblS_IncidentesAgrupacionCCDetMapping : EntityTypeConfiguration<tblS_IncidentesAgrupacionCCDet>
    {
        public tblS_IncidentesAgrupacionCCDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.esActivo).HasColumnName("esActivo");

            Property(x => x.idAgrupacionCC).HasColumnName("idAgrupacionCC");
            HasRequired(x => x.idAgrupacion).WithMany().HasForeignKey(y => y.idAgrupacionCC);

            ToTable("tblS_IncidentesAgrupacionCCDet");
        }
    }
}
