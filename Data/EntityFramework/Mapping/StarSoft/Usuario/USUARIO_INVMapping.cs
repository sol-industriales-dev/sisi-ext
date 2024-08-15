using Core.Entity.StarSoft.Usuario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.Usuario
{
    public class USUARIO_INVMapping : EntityTypeConfiguration<USUARIO_INV>
    {
        public USUARIO_INVMapping()
        {
            HasKey(x => new { x.USU_CODIGO, x.EMP_CODIGO });
            ToTable("USUARIO_INV");
        }
    }
}
