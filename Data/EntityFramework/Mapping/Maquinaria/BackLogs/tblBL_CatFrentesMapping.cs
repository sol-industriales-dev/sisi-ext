using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
using Core.Entity.Maquinaria.BackLogs;



namespace Data.EntityFramework.Mapping.Maquinaria.BackLogs
{
    public class tblBL_CatFrentesMapping : EntityTypeConfiguration<tblBL_CatFrentes>
    {
        public tblBL_CatFrentesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombreFrente).HasColumnName("nombreFrente");
            Property(x => x.idUsuarioAsignado).HasColumnName("idUsuarioAsignado");
            //Property(x => x.cc).HasColumnName("cc");
            //Property(x => x.folioPpto).HasColumnName("folioPpto");
            //Property(x => x.fechaAsignacion).HasColumnName("fechaAsignacion");
            //Property(x => x.avance).HasColumnName("avance");
            //Property(x => x.fechaPromesa).HasColumnName("fechaPromesa");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fecheModificacion).HasColumnName("fechaModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            HasRequired(x => x.UsuarioCreacion).WithMany().HasForeignKey(y => y.idUsuarioAsignado);

            ToTable("tblBL_CatFrentes");
        }
    }
}
