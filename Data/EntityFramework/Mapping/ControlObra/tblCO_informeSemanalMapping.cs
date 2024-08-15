using Core.Entity.ControlObra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.EntityFramework.Mapping.ControlObra
{
     class tblCO_informeSemanalMapping : EntityTypeConfiguration<tblCO_informeSemanal>
    {
         public tblCO_informeSemanalMapping()
         {
             HasKey(x => x.id);
             Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
             Property(x => x.cc).HasColumnName("cc");
             Property(x => x.fecha).HasColumnName("fecha");
             Property(x => x.periodo).HasColumnName("periodo");     
             Property(x => x.estatus).HasColumnName("estatus");

             Property(x => x.plantilla_id).HasColumnName("plantilla_id");
             HasRequired(x => x.plantilla).WithMany(x => x.informes).HasForeignKey(d => d.plantilla_id);


             ToTable("tblCO_informeSemanal");
         }
    }
}
