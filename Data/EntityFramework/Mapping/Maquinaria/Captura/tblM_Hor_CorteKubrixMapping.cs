using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura
{
    public class tblM_Hor_CorteKubrixMapping : EntityTypeConfiguration<tblM_Hor_CorteKubrix>
    {
        public tblM_Hor_CorteKubrixMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.acID).HasColumnName("acID");
            Property(x => x.fechaCorte).HasColumnName("fechaCorte");
            Property(x => x.usuarioCaptura).HasColumnName("usuarioCaptura");
            Property(x => x.fechaCaptura).HasColumnName("fechaCaptura");

            ToTable("tblM_Hor_CorteKubrix");
        }
    }
}
