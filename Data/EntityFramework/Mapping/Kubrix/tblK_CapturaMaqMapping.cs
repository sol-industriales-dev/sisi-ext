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
    class tblK_CapturaMaqMapping : EntityTypeConfiguration<tblK_CapturaMaq>
    {
        public tblK_CapturaMaqMapping(){
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.ccObra).HasColumnName("ccObra");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.economico).HasColumnName("economico");
            Property(x => x.turno).HasColumnName("turno");
            Property(x => x.horoInicial).HasColumnName("horoInicial");
            Property(x => x.horoFinal).HasColumnName("horoFinal");
            Property(x => x.paroClima).HasColumnName("paroClima");
            Property(x => x.hrsMtto).HasColumnName("hrsMtto");
            Property(x => x.horasTrab).HasColumnName("horasTrab");
            Property(x => x.horasProg).HasColumnName("horasProg");
            Property(x => x.horasEfectivas).HasColumnName("horasEfectivas");
            Property(x => x.eficiencia).HasColumnName("eficiencia");
            Property(x => x.consumo).HasColumnName("consumo");
            Property(x => x.grupoEquipo).HasColumnName("grupoEquipo");
            Property(x => x.rendTeorico).HasColumnName("rendTeorico");
            Property(x => x.rendReal).HasColumnName("rendReal");
            Property(x => x.rendimiento).HasColumnName("rendimiento");
            ToTable("tblK_CapturaMaq");
        }
    }
}
