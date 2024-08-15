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
    public class DocumentosResguardoMapping : EntityTypeConfiguration<tblM_DocumentosResguardos>
    {

        DocumentosResguardoMapping()
        {

            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.fechaSubido).HasColumnName("fechaSubido");
            Property(x => x.nombreArchivo).HasColumnName("nombreArchivo");
            Property(x => x.nombreRuta).HasColumnName("nombreRuta");
            Property(x => x.ResguardoID).HasColumnName("ResguardoID");
            Property(x => x.tipoArchivo).HasColumnName("tipoArchivo");
            Property(x => x.tipoResguardo).HasColumnName("tipoResguardo");

            ToTable("tblM_DocumentosResguardos");

        }
    }
}
