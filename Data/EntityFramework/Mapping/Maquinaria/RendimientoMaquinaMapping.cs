using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Data.EntityFramework.Mapping.Maquinaria
{
    class RendimientoMaquinaMapping : EntityTypeConfiguration<tblM_RendimientoMaquina>
    {
        public RendimientoMaquinaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.maquinaID).HasColumnName("maquinaID");            
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.rendimiento).HasColumnName("rendimiento");
            Property(x => x.modelo).HasColumnName("modelo");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.maquina).WithMany().HasForeignKey(y => y.maquinaID);
            ToTable("tblM_RendimientoMaquina");
        }
    }
}