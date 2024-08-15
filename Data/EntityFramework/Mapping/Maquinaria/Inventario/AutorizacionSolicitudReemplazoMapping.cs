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
    public class AutorizacionSolicitudReemplazoMapping : EntityTypeConfiguration<tblM_AutorizacionSolicitudReemplazo>
    {
        AutorizacionSolicitudReemplazoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.idAutorizaAsigna).HasColumnName("idAutorizaAsigna");
            Property(x => x.idAutorizaElabora).HasColumnName("idAutorizaElabora");
            Property(x => x.idAutorizaGerente).HasColumnName("idAutorizaGerente");
            Property(x => x.solicitudReemplazoEquipoID).HasColumnName("solicitudReemplazoEquipoID");

            Property(x => x.AutorizaAsigna).HasColumnName("AutorizaAsigna");
            Property(x => x.AutorizaElabora).HasColumnName("AutorizaElabora");
            Property(x => x.AutorizaGerente).HasColumnName("AutorizaGerente");
            Property(x => x.CadenaAsigna).HasColumnName("CadenaAsigna");
            Property(x => x.CadenaElabora).HasColumnName("CadenaElabora");
            Property(x => x.CadenaGerente).HasColumnName("CadenaGerente");
            Property(x => x.Comentarios).HasColumnName("Comentarios");
            Property(x => x.FechaAutorizacion).HasColumnName("FechaAutorizacion");
            Property(x => x.FechaElaboracion).HasColumnName("FechaElaboracion");

            HasRequired(x => x.SolicitudReemplazoEquipo).WithMany().HasForeignKey(y => y.solicitudReemplazoEquipoID );

            ToTable("tblM_AutorizacionSolicitudReemplazo");
        }
    }
}
