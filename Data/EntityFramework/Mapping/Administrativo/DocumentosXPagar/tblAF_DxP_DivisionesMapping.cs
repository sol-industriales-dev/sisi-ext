using Core.Entity.Administrativo.DocumentosXPagar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.DocumentosXPagar
{
    public class tblAF_DxP_DivisionesMapping: EntityTypeConfiguration<tblAF_DxP_Divisiones>
    {
        public tblAF_DxP_DivisionesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.abreviacion).HasColumnName("abreviacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblAF_DxP_Divisiones");
        }
    }
}
