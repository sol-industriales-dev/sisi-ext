using Core.Entity.Administrativo.RecursosHumanos.Mural;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.RecursosHumanos.Mural
{
    public class tblMural_DataMapping : EntityTypeConfiguration<tblMural_Data>
    {
        public tblMural_DataMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.datos).HasColumnName("datos");
            Property(x => x.titulo).HasColumnName("titulo");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.fecha).HasColumnName("fecha");

            ToTable("tblMural_Data");
        }
    }
}
