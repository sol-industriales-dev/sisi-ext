using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.BackLogs;

namespace Data.EntityFramework.Mapping.Maquinaria.BackLogs
{
    public class tblBL_DetFrentesMapping : EntityTypeConfiguration<tblBL_DetFrentes>
    {
        public tblBL_DetFrentesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idFrente).HasColumnName("idFrente");
            Property(x => x.idSeguimientoPpto).HasColumnName("idSeguimientoPpto");
            Property(x => x.fechaAsignacion).HasColumnName("fechaAsignacion");
            Property(x => x.avance).HasColumnName("avance");
            Property(x => x.idInspTMC).HasColumnName("idInspTMC");
            Property(x => x.esActivo).HasColumnName("esActivo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
        


            //TABLAS VIRTUALES
            HasRequired(x => x.lstCatFrentes).WithMany().HasForeignKey(y => y.idFrente);
            HasRequired(x => x.lstSeguimiento).WithMany().HasForeignKey(y => y.idSeguimientoPpto);
            HasRequired(x => x.lstInspeccionesTMC).WithMany().HasForeignKey(y => y.idInspTMC);

            ToTable("tblBL_DetFrentes");


        }
    }
}
