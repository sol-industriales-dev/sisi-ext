using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.Captura
{
    class FiniquitoFirmasMapping : EntityTypeConfiguration<tblRH_FiniquitoFirmas>
    {
        public FiniquitoFirmasMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.finiquitoID).HasColumnName("finiquitoID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.usuarioID).HasColumnName("usuarioID");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.ape_paterno).HasColumnName("ape_paterno");
            Property(x => x.ape_materno).HasColumnName("ape_materno");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.autorizando).HasColumnName("autorizando");
            Property(x => x.rechazado).HasColumnName("rechazado");
            Property(x => x.orden).HasColumnName("orden");
            ToTable("tblRH_FiniquitoFirmas");
        }
    }
}
