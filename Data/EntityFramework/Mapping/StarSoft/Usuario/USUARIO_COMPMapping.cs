using Core.Entity.StarSoft.Usuario;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Usuario
{
    public class USUARIO_COMPMapping : EntityTypeConfiguration<USUARIO_COMP>
    {
        public USUARIO_COMPMapping()
        {
            HasKey(x => new { x.USU_CODIGO, x.EMP_CODIGO});
            ToTable("USUARIO_COMP");
        }
    }
}
