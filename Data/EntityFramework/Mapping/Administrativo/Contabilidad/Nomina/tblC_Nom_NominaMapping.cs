using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Nomina
{

    public class tblC_Nom_NominaMapping : EntityTypeConfiguration<tblC_Nom_Nomina>
    {
        public tblC_Nom_NominaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.year).HasColumnName("year");
            Property(x => x.periodo).HasColumnName("periodo");
            Property(x => x.tipoNomina).HasColumnName("tipoNomina");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.nombreCC).HasColumnName("nombreCC");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.validada).HasColumnName("validada");
            Property(x => x.usuarioValidoId).HasColumnName("usuarioValidoId");
            Property(x => x.fechaValidacion).HasColumnName("fechaValidacion");
            Property(x => x.poliza).HasColumnName("poliza");
            Property(x => x.dirArchivo).HasColumnName("dirArchivo");
            Property(x => x.nombreArchivo).HasColumnName("nombreArchivo");
            Property(x => x.tipoRayaId).HasColumnName("tipoRayaId");
            Property(x => x.clasificacionCcId).HasColumnName("clasificacionCcId");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            HasRequired(x => x.usuarioCreacion).WithMany().HasForeignKey(y => y.usuarioCreacionId);
            HasRequired(x => x.usuarioModificacion).WithMany().HasForeignKey(y => y.usuarioModificacionId);
            HasOptional(x => x.usuarioValido).WithMany().HasForeignKey(y => y.usuarioValidoId);
            HasRequired(x => x.tipoRaya).WithMany().HasForeignKey(y => y.tipoRayaId);
            HasRequired(x => x.clasificacionCC).WithMany().HasForeignKey(y => y.clasificacionCcId);

            ToTable("tblC_Nom_Nomina");
        }
    }
}
