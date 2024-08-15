using Core.Entity.Encuestas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Encuestas.SubContratista
{
    public class EncuestaSubContratistaMapping: EntityTypeConfiguration<tblEN_EncuestaSubContratista>
    {
        public EncuestaSubContratistaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.titulo).HasColumnName("titulo");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.creadorID).HasColumnName("creadorID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.tipoEncuesta).HasColumnName("tipoEncuesta");
            HasRequired(x => x.creador).WithMany().HasForeignKey(y => y.creadorID);
            ToTable("tblEN_EncuestaSubContratista");
        }
    }
}
