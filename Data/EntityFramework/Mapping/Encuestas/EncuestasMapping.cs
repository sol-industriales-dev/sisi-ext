using Core.Entity.Encuestas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Encuestas
{
    public class EncuestasMapping : EntityTypeConfiguration<tblEN_Encuesta>
    {
        public EncuestasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.titulo).HasColumnName("titulo");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.creadorID).HasColumnName("creadorID");
            Property(x => x.departamentoID).HasColumnName("departamentoID");
            HasRequired(x => x.departamento).WithMany().HasForeignKey(y => y.departamentoID);
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.estatusAutoriza).HasColumnName("estatusAutoriza");
            Property(x => x.tipo).HasColumnName("tipo");
            Property(x => x.telefonica).HasColumnName("telefonica");
            Property(x => x.notificacion).HasColumnName("notificacion");
            Property(x => x.papel).HasColumnName("papel");
            Property(x => x.soloLectura).HasColumnName("soloLectura");
            HasRequired(x => x.departamento).WithMany().HasForeignKey(y => y.departamentoID);
            ToTable("tblEN_Encuesta");
        }
    }
}
