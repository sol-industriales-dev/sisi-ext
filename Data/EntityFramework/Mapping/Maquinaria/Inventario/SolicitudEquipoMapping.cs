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
    public class SolicitudEquipoMapping : EntityTypeConfiguration<tblM_SolicitudEquipo>
    {
        SolicitudEquipoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.CC).HasColumnName("CC");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.cantidad).HasColumnName("cantidad");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.fechaElaboracion).HasColumnName("fechaElaboracion");
            Property(x => x.Estatus).HasColumnName("Estatus");
            Property(x => x.HorasTotales).HasColumnName("HorasTotales");
            Property(x => x.ArranqueObra).HasColumnName("ArranqueObra");
            Property(x => x.EstatdoSolicitud).HasColumnName("EstatdoSolicitud");
            Property(x => x.condicionInicial).HasColumnName("condicionInicial");
            Property(x => x.condicionActual).HasColumnName("condicionActual");
            Property(x => x.justificacion).HasColumnName("justificacion");
            Property(x => x.link).HasColumnName("link");
            
            ToTable("tblM_SolicitudEquipo");
        }
    }
}
