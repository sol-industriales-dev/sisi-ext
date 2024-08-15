using Core.Entity.StarSoft.Almacen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Requisiciones
{
    public class STKARTMapping : EntityTypeConfiguration<STKART>
    {
        public STKARTMapping()
        {
            HasKey(x => new { x.STALMA, x.STCODIGO });
            ToTable("STKART");
        }
    }
}
