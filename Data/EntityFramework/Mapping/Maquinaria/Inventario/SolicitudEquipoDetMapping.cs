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
    public class SolicitudEquipoDetMapping : EntityTypeConfiguration<tblM_SolicitudEquipoDet>
    {
        SolicitudEquipoDetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.tipoMaquinariaID).HasColumnName("tipoMaquinariaID");
            Property(x => x.grupoMaquinariaID).HasColumnName("grupoMaquinariaID");
            Property(x => x.modeloEquipoID).HasColumnName("modeloEquipoID");
            Property(x => x.horas).HasColumnName("horas");
            Property(x => x.fechaInicio).HasColumnName("fechaInicio");
            Property(x => x.fechaFin).HasColumnName("fechaFin");
            Property(x => x.prioridad).HasColumnName("prioridad");
            Property(x => x.solicitudEquipoID).HasColumnName("solicitudEquipoID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.Comentario).HasColumnName("Comentario");
            Property(x => x.tipoUtilizacion).HasColumnName("tipoUtilizacion");

            HasRequired(x => x.SolicitudEquipo).WithMany().HasForeignKey(y => y.solicitudEquipoID);
            HasRequired(x => x.GrupoMaquinaria).WithMany().HasForeignKey(y => y.grupoMaquinariaID);
            HasRequired(x => x.TipoMaquinaria).WithMany().HasForeignKey(y => y.tipoMaquinariaID);
            HasRequired(x => x.ModeloEquipo).WithMany().HasForeignKey(y => y.modeloEquipoID);

            ToTable("tblM_SolicitudEquipoDet");
        }
    }
}
