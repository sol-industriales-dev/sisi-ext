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
    public class ResponsablesMapping : EntityTypeConfiguration<tblSA_Responsables>
    {
        public ResponsablesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.minutaID).HasColumnName("minutaID");
            Property(x => x.actividadID).HasColumnName("actividadID");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.usuarioText).HasColumnName("usuario");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.usuarioID);
            HasRequired(x => x.minuta).WithMany().HasForeignKey(y => y.minutaID);
            HasRequired(x => x.actividad).WithMany().HasForeignKey(y => y.actividadID);
            ToTable("tblSA_Responsables");
        }
    }
}
