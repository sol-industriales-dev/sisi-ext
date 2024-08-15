using Core.Entity.Administrativo.DocumentosXPagar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_TipoFechaVencimientoMapping : EntityTypeConfiguration<tblAF_DxP_TipoFechaVencimiento>
    {
        public tblAF_DxP_TipoFechaVencimientoMapping()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Descripcion).HasColumnName("descripcion");
            ToTable("tblAF_DxP_TipoFechaVencimiento");
        }
    }
}