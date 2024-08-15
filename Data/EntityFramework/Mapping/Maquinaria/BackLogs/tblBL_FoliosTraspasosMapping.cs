using Core.Entity.Maquinaria.BackLogs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.BackLogs
{
    public class tblBL_FoliosTraspasosMapping : EntityTypeConfiguration<tblBL_FoliosTraspasos>
    {
        public tblBL_FoliosTraspasosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idBL).HasColumnName("idBL");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.almacenID).HasColumnName("almacenID");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.almDestinoID).HasColumnName("almDestinoID");
            Property(x => x.ccDestino).HasColumnName("ccDestino");
            Property(x => x.folioTraspaso).HasColumnName("folioTraspaso");
            Property(x => x.esTraspasoCompleto).HasColumnName("esTraspasoCompleto");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblBL_FoliosTraspasos");
        }
    }
}
