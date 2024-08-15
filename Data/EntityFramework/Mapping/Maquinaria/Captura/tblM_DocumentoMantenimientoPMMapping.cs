using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    class tblM_DocumentoMantenimientoPMMapping : EntityTypeConfiguration<tblM_DocumentoMantenimientoPM>
    {
        public tblM_DocumentoMantenimientoPMMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblM_DocumentoMantenimientoPM");
        }
    }
}
