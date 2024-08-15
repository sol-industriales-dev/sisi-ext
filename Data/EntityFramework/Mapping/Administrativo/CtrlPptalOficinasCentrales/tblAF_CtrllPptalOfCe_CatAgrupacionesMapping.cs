using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Administrativo.CtrlPptalOficinasCentrales;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.EntityFramework.Mapping.Administrativo.CtrlPptalOficinasCentrales
{
    public class tblAF_CtrllPptalOfCe_CatAgrupacionesMapping : EntityTypeConfiguration<tblAF_CtrllPptalOfCe_CatAgrupaciones>
    {
        public tblAF_CtrllPptalOfCe_CatAgrupacionesMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            ToTable("tblAF_CtrllPptalOfCe_CatAgrupaciones");
        }
    }
}
