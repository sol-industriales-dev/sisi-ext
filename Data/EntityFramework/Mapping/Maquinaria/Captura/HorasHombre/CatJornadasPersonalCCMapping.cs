using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Captura.HorasHombre
{
    public class CatJornadasPersonalCCMapping : EntityTypeConfiguration<tblM_CatJornadasPersonalCC>
    {
        public CatJornadasPersonalCCMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("nombreEmpleado");
            Property(x => x.diasSemana).HasColumnName("numEmpleado");
            Property(x => x.hrsTrabajadasDias).HasColumnName("categoriaTrabajo");

            ToTable("tblM_CatJornadasPersonalCC");
        }

    }
}
