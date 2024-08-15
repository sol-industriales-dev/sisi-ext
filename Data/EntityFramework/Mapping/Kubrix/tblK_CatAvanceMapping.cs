using Core.Entity.Kubrix;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Kubrix
{
    public class tblK_CatAvanceMapping : EntityTypeConfiguration<tblK_CatAvance>
    {
        public tblK_CatAvanceMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.rutaArchivo).HasColumnName("rutaArchivo");
            Property(x => x.nombreArchivo).HasColumnName("nombreArchivo");
            Property(x => x.fecha).HasColumnName("fecha");
            ToTable("tblK_CatAvance", "Kubrix");
        }
    }
}
