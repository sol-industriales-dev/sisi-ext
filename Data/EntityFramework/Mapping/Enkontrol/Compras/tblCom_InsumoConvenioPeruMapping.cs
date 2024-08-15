using Core.Entity.Enkontrol.Compras;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras
{
    public class tblCom_InsumoConvenioPeruMapping : EntityTypeConfiguration<tblCom_InsumosConvenioPeru>
    {
        public tblCom_InsumoConvenioPeruMapping()
        {
            HasKey(x => x.id);
            ToTable("tblCom_InsumosConvenioPeru");
        }
    }
}
