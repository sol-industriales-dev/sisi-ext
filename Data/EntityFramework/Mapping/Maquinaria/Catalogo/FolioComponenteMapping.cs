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
    public class FolioComponenteMapping : EntityTypeConfiguration<tblM_FolioComponente>
    {
        public FolioComponenteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.modeloID).HasColumnName("modeloID");
            Property(x => x.conjuntoID).HasColumnName("conjuntoID");
            Property(x => x.subConjuntoID).HasColumnName("subConjuntoID");
            Property(x => x.cc).HasColumnName("cc");
            //Property(x => x.posicionID).HasColumnName("posicionID");
            Property(x => x.Folio).HasColumnName("Folio");
            Property(x => x.prefijo).HasColumnName("prefijo");
            ToTable("tblM_FolioComponente");
        }
    }
}
