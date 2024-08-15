using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura.Conciliacion
{
    class AutorizacionesConciliacionHorometrosMapping : EntityTypeConfiguration<tblM_AutorizaConciliacionHorometros>
    {
        public AutorizacionesConciliacionHorometrosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.autorizaAdmin).HasColumnName("autorizaAdmin");
            Property(x => x.autorizaGerenteID).HasColumnName("autorizaGerenteID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.firmaAdmin).HasColumnName("firmaAdmin");
            Property(x => x.firmaGerente).HasColumnName("firmaGerente");
            Property(x => x.pendienteAdmin).HasColumnName("pendienteAdmin");
            Property(x => x.pendienteGerente).HasColumnName("pendienteGerente");
            Property(x => x.conciliacionID).HasColumnName("conciliacionID");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.autorizaDirector).HasColumnName("autorizaDirector");
            Property(x => x.firmaDirector).HasColumnName("firmaDirector");
            Property(x => x.autorizando).HasColumnName("autorizando");
            Property(x => x.pendienteDirector).HasColumnName("pendienteDirector");
            Property(x => x.comentario).HasColumnName("comentario");

            ToTable("tblM_AutorizaConciliacionHorometros");
        }
    }
}


