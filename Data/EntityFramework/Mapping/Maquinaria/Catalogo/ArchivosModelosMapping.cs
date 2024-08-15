using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Catalogo
{

    public class ArchivosModelosMapping : EntityTypeConfiguration<tblM_ArchivosModelos>
    {
        ArchivosModelosMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.modeloID).HasColumnName("NotaCreditoID");
            Property(x => x.nombreArchivo).HasColumnName("nombreArchivo");
            Property(x => x.RutaArchivo).HasColumnName("rutaArchivo");
            Property(x => x.usuario).HasColumnName("usuario");
            Property(x => x.fechaCreacion).HasColumnName("FechaSubida");

            ToTable("tblM_ArchivosModelos");
        }
    }
}
