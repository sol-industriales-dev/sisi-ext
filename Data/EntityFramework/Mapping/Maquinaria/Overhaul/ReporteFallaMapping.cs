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
    public class ReporteFallaMapping : EntityTypeConfiguration<tblM_ReporteFalla>
    {
        public ReporteFallaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.maquinaID).HasColumnName("maquinaID");
            Property(x => x.fechaReporte).HasColumnName("fechaReporte");
            Property(x => x.fechaParo).HasColumnName("fechaParo");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.descripcionFalla).HasColumnName("descripcionFalla");
            Property(x => x.fallaComponente).HasColumnName("fallaComponente");
            Property(x => x.causaFalla).HasColumnName("causaFalla");
            Property(x => x.diagnosticosAplicados).HasColumnName("diagnosticosAplicados");

            Property(x => x.tipoReparacion).HasColumnName("tipoReparacion");
            Property(x => x.reparaciones).HasColumnName("reparaciones");
            Property(x => x.destino).HasColumnName("destino");
            Property(x => x.horometroReporte).HasColumnName("horometroReporte");
            Property(x => x.realiza).HasColumnName("realiza");
            Property(x => x.revisa).HasColumnName("revisa");
            Property(x => x.procedencia).HasColumnName("procedencia");
            Property(x => x.fechaAlta).HasColumnName("fechaAlta");
            Property(x => x.estatus).HasColumnName("estatus");

            //HasRequired(x => x.maquina).WithMany().HasForeignKey(x => x.maquinaID);
            HasOptional(x => x.lstArchivos).WithMany().HasForeignKey(x => x.id); ;
            ToTable("tblM_ReporteFalla");
        }
    }
}