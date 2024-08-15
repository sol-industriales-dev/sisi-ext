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
    public class tblBL_MotivoCancelacionReqMapping: EntityTypeConfiguration<tblBL_MotivoCancelacionReq>
    {
        public tblBL_MotivoCancelacionReqMapping(){
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idBL).HasColumnName("idBL");
            Property(x => x.idUsuario).HasColumnName("idUsuario");
            Property(x => x.motivo).HasColumnName("motivo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            HasRequired(x => x.lstUsuarios).WithMany().HasForeignKey(y => y.idUsuario);

            ToTable("tblBL_MotivoCancelacionReq");
        }
    }
}
