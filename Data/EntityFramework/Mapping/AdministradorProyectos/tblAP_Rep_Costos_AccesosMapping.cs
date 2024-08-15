using Core.Entity.AdministradorProyectos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.AdministradorProyectos
{
    public class tblAP_Rep_Costos_AccesosMapping : EntityTypeConfiguration<tblAP_Rep_Costos_Accesos>
    {
        public tblAP_Rep_Costos_AccesosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.cc).HasColumnName("cc");
            ToTable("tblAP_Rep_Costos_Accesos");
        }
    }
}
