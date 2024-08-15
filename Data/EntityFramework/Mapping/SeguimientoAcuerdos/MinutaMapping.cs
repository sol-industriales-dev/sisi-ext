using Core.Entity.SeguimientoAcuerdos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SeguimientoAcuerdos
{
    public class MinutaMapping : EntityTypeConfiguration<tblSA_Minuta>
    {
        public MinutaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.proyecto).HasColumnName("proyecto");
            Property(x => x.titulo).HasColumnName("titulo");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.creadorID).HasColumnName("creadorID");
            HasRequired(x => x.creador).WithMany().HasForeignKey(y => y.creadorID);
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaCompromiso).HasColumnName("fechaCompromiso");
            ToTable("tblSA_Minuta");
        }
    }
}
