using Core.Entity.Enkontrol.Compras.OrdenCompra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.OrdenCompra
{
    class tblCom_Retenciones_CatMapping : EntityTypeConfiguration<tblCom_Retenciones_Cat>
    {
        public tblCom_Retenciones_CatMapping()
        {
            HasKey(x => x.id_cpto);
            
            ToTable("tblCom_Retenciones_Cat");
        }
    }
}
