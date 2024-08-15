using Core.Entity.StarSoft.Almacen;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Almacen
{
    public class MoResMesMapping : EntityTypeConfiguration<MoResMes>
    {
        public MoResMesMapping()
        {
            HasKey(x => new { x.SMALMA, x.SMCODIGO, x.SMMESPRO });
            ToTable("MoResMes");
        }
    }
}
