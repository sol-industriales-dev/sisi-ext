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
    public class tblC_Nom_UsuarioValidaMapping : EntityTypeConfiguration<tblC_Nom_UsuarioValida>
    {
        public tblC_Nom_UsuarioValidaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioId).HasColumnName("usuarioId");
            Property(x => x.estatus).HasColumnName("estatus");
            HasRequired(x => x.usuario).WithMany().HasForeignKey(y => y.usuarioId);
            ToTable("tblC_Nom_UsuarioValida");
        }
    }
}
