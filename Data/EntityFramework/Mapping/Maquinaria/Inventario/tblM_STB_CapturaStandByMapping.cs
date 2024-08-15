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
    public class tblM_STB_CapturaStandByMapping : EntityTypeConfiguration<tblM_STB_CapturaStandBy>
    {
        public tblM_STB_CapturaStandByMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.Economico).HasColumnName("Economico");
            Property(x => x.noEconomicoID).HasColumnName("noEconomicoID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.usuarioCapturaID).HasColumnName("usuarioCapturaID");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.usuarioAutorizaID).HasColumnName("usuarioAutorizaID");
            Property(x => x.fechaAutoriza).HasColumnName("fechaAutoriza");
            Property(x => x.usuarioLiberaID).HasColumnName("usuarioLiberaID");
            Property(x => x.fechaLibera).HasColumnName("fechaLibera");
            Property(x => x.ccActual).HasColumnName("ccActual");
            Property(x => x.comentarioJustificacion).HasColumnName("comentarioJustificacion");
            Property(x => x.comentarioValidacion).HasColumnName("comentarioValidacion");
            Property(x => x.comentarioLiberacion).HasColumnName("comentarioLiberacion");
            Property(x => x.evidenciaJustificacion).HasColumnName("evidenciaJustificacion");

            HasRequired(x => x.noEconomico).WithMany().HasForeignKey(y => y.noEconomicoID);
            HasRequired(x => x.usuarioCaptura).WithMany().HasForeignKey(y => y.usuarioCapturaID);
            HasRequired(x => x.usuarioAutoriza).WithMany().HasForeignKey(y => y.usuarioAutorizaID);
            HasRequired(x => x.usuarioLibera).WithMany().HasForeignKey(y => y.usuarioLiberaID);

            ToTable("tblM_STB_CapturaStandBy");
        }

    }
}
