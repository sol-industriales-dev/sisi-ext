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
    public class InteresadosMapping : EntityTypeConfiguration<tblSA_Interesados>
    {
        public InteresadosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.minutaID).HasColumnName("minutaID");
            Property(x => x.actividadID).HasColumnName("actividadID");
            Property(x => x.interesadoID).HasColumnName("interesadoID");
            Property(x => x.interesado).HasColumnName("interesado");
            HasRequired(x => x.minuta).WithMany(x => x.interesados).HasForeignKey(y => y.minutaID);
            HasRequired(x => x.actividad).WithMany().HasForeignKey(y => y.actividadID);
            ToTable("tblSA_Interesados");
        }
    }
}
