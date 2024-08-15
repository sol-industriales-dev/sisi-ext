using Core.Entity.Administrativo.Facultamiento;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Facultamiento
{
    public class AuthMapping : EntityTypeConfiguration<tblFa_CatAuth> 
    {
        public AuthMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idFacultamiento).HasColumnName("idFacultamiento");
            Property(x => x.idUsuario).HasColumnName("idUsuario");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.fechaFirma).HasColumnName("fechaFirma");
            Property(x => x.firma).HasColumnName("firma");
            Property(x => x.auth).HasColumnName("auth");
            ToTable("tblFa_CatAuth");
        }
    }
}
