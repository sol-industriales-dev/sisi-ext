using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Overhaul
{
    public class ReporteFallaComponenteMapping : EntityTypeConfiguration<tblM_ReporteFalla_Componente>
    {
        public ReporteFallaComponenteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idReporteFalla).HasColumnName("idReporteFalla");
            Property(x => x.Conjunto).HasColumnName("Conjunto");
            Property(x => x.Subconjunto).HasColumnName("Subconjunto");
            Property(x => x.Componente).HasColumnName("Componente");
            Property(x => x.Fecha).HasColumnName("Fecha");
            Property(x => x.Horas).HasColumnName("Horas");
            Property(x => x.Parte).HasColumnName("Parte");
            Property(x => x.cc).HasColumnName("cc");
            HasRequired(x => x.Reporte).WithMany().HasForeignKey(x => x.idReporteFalla);
            ToTable("tblM_ReporteFalla_Componente");
        }
    }
}
