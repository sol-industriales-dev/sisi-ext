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
    public class tblBL_BitacoraEstatusBLMapping : EntityTypeConfiguration<tblBL_BitacoraEstatusBL>
    {
        public tblBL_BitacoraEstatusBLMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idBL).HasColumnName("idBL");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.diasTranscurridos).HasColumnName("diasTranscurridos");
            Property(x => x.idEstatus).HasColumnName("idEstatus");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblBL_BitacoraEstatusBL");
        }
    }
}
