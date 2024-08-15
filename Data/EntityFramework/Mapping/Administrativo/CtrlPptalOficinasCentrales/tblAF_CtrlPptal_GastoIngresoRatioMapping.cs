using Core.Entity.Administrativo.CtrlPptalOficinasCentrales;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.CtrlPptalOficinasCentrales
{
    public class tblAF_CtrlPptal_GastoIngresoRatioMapping : EntityTypeConfiguration<tblAF_CtrlPptal_GastoIngresoRatio>
    {
        public tblAF_CtrlPptal_GastoIngresoRatioMapping()
        {
            HasKey(x => x.id);
            ToTable("tblAF_CtrlPptal_GastoIngresoRatio");
        }
    }
}
