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
    public class AnexoContratoMapping : EntityTypeConfiguration<tblX_AnexoContrato>
    {
        public AnexoContratoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.contratoID).HasColumnName("contratoID");
            Property(x => x.tipoAnexo).HasColumnName("tipoAnexo");
            Property(x => x.rutaArchivo).HasColumnName("rutaArchivo");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");
            Property(x => x.estatus).HasColumnName("estatus");

            ToTable("tblX_AnexoContrato");
        }
    }
}
