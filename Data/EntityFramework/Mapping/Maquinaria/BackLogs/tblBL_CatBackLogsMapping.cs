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
    public class tblBL_CatBackLogsMapping : EntityTypeConfiguration<tblBL_CatBackLogs>
    {
        public tblBL_CatBackLogsMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.folioBL).HasColumnName("folioBL");
            Property(x => x.fechaInspeccion).HasColumnName("fechaInspeccion");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.noEconomico).HasColumnName("noEconomico");
            Property(x => x.horas).HasColumnName("horas");
            Property(x => x.idSubconjunto).HasColumnName("idSubconjunto");
            Property(x => x.parte).HasColumnName("parte");
            Property(x => x.manoObra).HasColumnName("manoObra");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.totalMX).HasColumnName("totalMX");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.idEstatus).HasColumnName("idEstatus");
            Property(x => x.tipoBL).HasColumnName("tipoBL");
            Property(x => x.presupuestoEstimado).HasColumnName("presupuestoEstimado");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCreacionBL).HasColumnName("fechaCreacionBL");
            Property(x => x.fechaModificacionBL).HasColumnName("fechaModificacionBL");
            Property(x => x.idSegPpto).HasColumnName("idSegPpto");
            Property(x => x.esLiberado).HasColumnName("esLiberado");
            Property(x => x.idUsuarioResponsable).HasColumnName("idUsuarioResponsable");
            Property(x => x.fechaLiberadoBL).HasColumnName("fechaLiberadoBL");
            Property(x => x.fechaInstaladoBL).HasColumnName("fechaInstaladoBL");

            //HasRequired(x => x.lstBackLogs).WithMany().HasForeignKey(y => y.idSegPpto);
            HasRequired(x => x.subconjunto).WithMany().HasForeignKey(y => y.idSubconjunto);

            ToTable("tblBL_CatBackLogs");
        }
    }
}
