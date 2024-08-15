using Core.Entity.SubContratistas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.SubContratistas
{
    public class SubContratistaMapping : EntityTypeConfiguration<tblX_SubContratista>
    {
        public SubContratistaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.numeroProveedor).HasColumnName("numeroProveedor");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.direccion).HasColumnName("direccion");
            Property(x => x.nombreCorto).HasColumnName("nombreCorto");
            Property(x => x.codigoPostal).HasColumnName("codigoPostal");
            Property(x => x.rfc).HasColumnName("rfc");
            Property(x => x.correo).HasColumnName("correo");
            Property(x => x.divisionId).HasColumnName("divisionId");
            Property(x => x.fisica).HasColumnName("fisica");
            Property(x => x.pendienteValidacion).HasColumnName("pendienteValidacion");
            Property(x => x.estatus).HasColumnName("estatus");

            HasOptional(x => x.division).WithMany().HasForeignKey(y => y.divisionId);
            HasRequired(x => x.tipoBloqueo).WithMany().HasForeignKey(y => y.tipoBloqueoId);

            ToTable("tblX_SubContratista");
        }
    }
}
