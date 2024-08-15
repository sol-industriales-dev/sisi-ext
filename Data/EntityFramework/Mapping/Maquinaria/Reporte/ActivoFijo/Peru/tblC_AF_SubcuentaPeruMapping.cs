using Core.Entity.Maquinaria.Reporte.ActivoFijo.Peru;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Reporte.ActivoFijo.Peru
{
    public class tblC_AF_SubcuentaPeruMapping : EntityTypeConfiguration<tblC_AF_SubcuentaPeru>
    {
        public tblC_AF_SubcuentaPeruMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cuentaId).HasColumnName("cuentaId");
            Property(x => x.scta).HasColumnName("scta");
            Property(x => x.sscta).HasColumnName("sscta");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.esMaquinaria).HasColumnName("esMaquinaria");
            Property(x => x.tipoMaquinaId).HasColumnName("tipoMaquinaId");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.cuentaPeru).WithMany().HasForeignKey(y => y.cuentaId);
            HasOptional(x => x.tipoMaquina).WithMany().HasForeignKey(y => y.tipoMaquinaId);
            ToTable("tblC_AF_SubcuentaPeru");
        }
    }
}
