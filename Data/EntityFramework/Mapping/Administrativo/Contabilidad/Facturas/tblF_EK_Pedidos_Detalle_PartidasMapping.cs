using Core.Entity.Administrativo.Contabilidad.Facturas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Facturas
{
    public class tblF_EK_Pedidos_Detalle_PartidasMapping : EntityTypeConfiguration<tblF_EK_Pedidos_Detalle_Partidas>
    {
        public tblF_EK_Pedidos_Detalle_PartidasMapping()
        {
            HasKey(e => e.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblF_EK_Pedidos_Detalle_Partidas");
        }
    }
}
