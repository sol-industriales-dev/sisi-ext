using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class MaquinariaAceitesLubricantesMapping : EntityTypeConfiguration<tblM_MaquinariaAceitesLubricantes>
    {
        public MaquinariaAceitesLubricantesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Economico).HasColumnName("Economico");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.Fecha).HasColumnName("Fecha");
            Property(x => x.Turno).HasColumnName("Turno");
            Property(x => x.Horometro).HasColumnName("Horometro");
            Property(x => x.Rotacion).HasColumnName("Rotacion");
            Property(x => x.Sopleteo).HasColumnName("Sopleteo");
            Property(x => x.AK).HasColumnName("AK");
            Property(x => x.Lubricacion).HasColumnName("Lubricacion");
            Property(x => x.Antifreeze).HasColumnName("Antifreeze");
            Property(x => x.MotorId).HasColumnName("MotorId");
            Property(x => x.MotorVal).HasColumnName("MotorVal");
            Property(x => x.TransmisionID).HasColumnName("TransmisionID");
            Property(x => x.TransmisionVal).HasColumnName("TransmisionVal");
            Property(x => x.HidraulicoID).HasColumnName("HidraulicoID");
            Property(x => x.HidraulicoVal).HasColumnName("HidraulicoVal");
            Property(x => x.DiferencialId).HasColumnName("DiferencialId");
            Property(x => x.DiferencialVal).HasColumnName("DiferencialVal");
            Property(x => x.MFTIzqId).HasColumnName("MFTIzqId");
            Property(x => x.MFTDerId).HasColumnName("MFTDerId");
            Property(x => x.MFTIzqVal).HasColumnName("MFTIzqVal");
            Property(x => x.MFTDerVal).HasColumnName("MFTDerVal");
            Property(x => x.MDIzqID).HasColumnName("MDIzqID");
            Property(x => x.MDDerID).HasColumnName("MDDerID");
            Property(x => x.MDIzqVal).HasColumnName("MDIzqVal");
            Property(x => x.MDDerVal).HasColumnName("MDDerVal");
            Property(x => x.MotorVal).HasColumnName("MotorVal");
            Property(x => x.DirId).HasColumnName("DirId");
            Property(x => x.DirVal).HasColumnName("DirVal");
            Property(x => x.Grasa).HasColumnName("Grasa");
            Property(x => x.Firma).HasColumnName("Firma");

            Property(x => x.otros1).HasColumnName("otros1");
            Property(x => x.otroId1).HasColumnName("otroId1");
            Property(x => x.otros2).HasColumnName("otros2");
            Property(x => x.otroId2).HasColumnName("otroId2");
            Property(x => x.otros3).HasColumnName("otros3");
            Property(x => x.otroId3).HasColumnName("otroId3");
            Property(x => x.otros4).HasColumnName("otros4");
            Property(x => x.otroId4).HasColumnName("otroId4");

            ToTable("tblM_MaquinariaAceitesLubricantes");
        }
    }
}
