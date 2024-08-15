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
    public class ParticipanteMapping : EntityTypeConfiguration<tblSA_Participante>
    {
        public ParticipanteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.minutaID).HasColumnName("minutaID");
            Property(x => x.participanteID).HasColumnName("participanteID");
            Property(x => x.participante).HasColumnName("participante");
            HasRequired(x => x.minuta).WithMany(x => x.participantes).HasForeignKey(y => y.minutaID);
            ToTable("tblSA_Participante");
        }
    }
}
