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
    public class ReporteFallaReparacionMapping : EntityTypeConfiguration<tblM_ReporteFalla_Reparacion>
    {
        public ReporteFallaReparacionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idReporteFalla).HasColumnName("idReporteFalla");
            Property(x => x.Tipo).HasColumnName("Tipo");
            Property(x => x.Grupo).HasColumnName("Grupo");
            Property(x => x.Insumo).HasColumnName("Insumo");
            Property(x => x.Bitacora).HasColumnName("Bitacora");
            HasRequired(x => x.Reporte).WithMany().HasForeignKey(x => x.idReporteFalla);
            ToTable("tblM_ReporteFalla_Reparacion");
        }
    }
}
