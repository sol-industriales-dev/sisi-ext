using Core.Entity.Administrativo.CtrlPptalOficinasCentrales;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.CtrlPptalOficinasCentrales
{
    public class tblAF_CtrlPptalOfCe_BaseConceptosMapping : EntityTypeConfiguration<tblAF_CtrlPptalOfCe_BaseConceptos>
    {
        public tblAF_CtrlPptalOfCe_BaseConceptosMapping()
        {
            HasKey(x => x.id);
            ToTable("tblAF_CtrlPptalOfCe_BaseConceptos");
        }
    }
}
