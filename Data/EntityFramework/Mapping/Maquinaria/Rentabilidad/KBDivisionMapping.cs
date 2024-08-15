using Core.Entity.Maquinaria.Rentabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Rentabilidad
{
    public class KBDivisionMapping : EntityTypeConfiguration<tblM_KBDivision>
    {
        KBDivisionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("");
            Property(x => x.division).HasColumnName("division");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            ToTable("tblM_KBDivision");

        }

    }
}
