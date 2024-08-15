using Core.Entity.AdministradorProyectos.CGP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.AdministradorProyectos.CGP
{
    public class MenuArchivosMapping : EntityTypeConfiguration<tblAP_CGP_MenuArchivos>
    {
        public MenuArchivosMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("Id");
            Property(x => x.IdMenu).HasColumnName("IdMenu");
            Property(x => x.DirArchivos).HasColumnName("DirArchivos");
            Property(x => x.esActivo).HasColumnName("esActivo");
            ToTable("tblAP_CGP_MenuArchivos");
        }
    }
}
