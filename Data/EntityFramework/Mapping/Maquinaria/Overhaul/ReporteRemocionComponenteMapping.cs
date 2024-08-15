using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Overhaul
{
    public class ReporteRemocionComponenteMapping : EntityTypeConfiguration<tblM_ReporteRemocionComponente>
    {
        ReporteRemocionComponenteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.fechaRemocion).HasColumnName("fechaRemocion");
            Property(x => x.componenteRemovidoID).HasColumnName("componenteRemovidoID");
            Property(x => x.componenteInstaladoID).HasColumnName("componenteInstaladoID");
            Property(x => x.maquinaID).HasColumnName("maquinaID");
            Property(x => x.areaCuenta).HasColumnName("areaCuenta");
            Property(x => x.motivoRemocionID).HasColumnName("motivoRemocionID");
            Property(x => x.destinoID).HasColumnName("destinoID");
            Property(x => x.comentario).HasColumnName("comentario");
            Property(x => x.garantia).HasColumnName("garantia");
            Property(x => x.empresaResponsable).HasColumnName("empresaResponsable");
            Property(x => x.personal).HasColumnName("personal");
            Property(x => x.imgComponenteRemovido).HasColumnName("imgComponenteRemovido");
            Property(x => x.imgComponenteInstalado).HasColumnName("imgComponenteInstalado");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.empresaInstala).HasColumnName("empresaInstala");
            Property(x => x.fechaInstalacionCRemovido).HasColumnName("fechaInstalacionCRemovido");
            Property(x => x.fechaInstalacionCInstalado).HasColumnName("fechaInstalacionCInstalado");
            Property(x => x.fechaUltimaReparacion).HasColumnName("fechaUltimaReparacion");
            Property(x => x.fechaVoBo).HasColumnName("fechaVoBo");
            Property(x => x.fechaEnvio).HasColumnName("fechaEnvio");
            Property(x => x.fechaAutorizacion).HasColumnName("fechaAutorizacion");
            Property(x => x.realiza).HasColumnName("realiza");
            Property(x => x.horasComponente).HasColumnName("horasComponente");
            Property(x => x.horasMaquina).HasColumnName("horasMaquina");
            Property(x => x.JsonEvidencia).HasColumnName("JsonEvidencia");
            Property(x => x.trackID).HasColumnName("trackID");

            HasRequired(x => x.componenteRemovido).WithMany().HasForeignKey(x => x.componenteRemovidoID);
            HasRequired(x => x.componenteInstalado).WithMany().HasForeignKey(x => x.componenteInstaladoID);
            HasRequired(x => x.maquina).WithMany().HasForeignKey(x => x.maquinaID);
            HasRequired(x => x.destino).WithMany().HasForeignKey(x => x.destinoID);

            ToTable("tblM_ReporteRemocionComponente");
        }
    }
}


