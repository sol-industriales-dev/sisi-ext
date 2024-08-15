using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.EntityFramework.Mapping.Principal.Usuarios
{
    public class DepartamentoMapping : EntityTypeConfiguration<tblP_Departamento>
    {
        public DepartamentoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.esDepartamento).HasColumnName("esDepartamento");
            ToTable("tblP_Departamento");
        }
    }
}
