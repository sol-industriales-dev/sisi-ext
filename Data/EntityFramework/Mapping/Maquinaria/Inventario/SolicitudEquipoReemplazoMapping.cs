using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Inventario
{
    public class SolicitudEquipoReemplazoMapping : EntityTypeConfiguration<tblM_SolicitudReemplazoEquipo>
    {
        SolicitudEquipoReemplazoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.fechaElaboracion).HasColumnName("fechaElaboracion");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            ToTable("tblM_SolicitudReemplazoEquipo");
        }
    }
}
