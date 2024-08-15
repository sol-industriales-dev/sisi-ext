using Core.Entity.ControlObra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.ControlObra
{
    public class tblCO_ADP_EvaluacionReqMapping : EntityTypeConfiguration<tblCO_ADP_EvaluacionReq>
    {
        public tblCO_ADP_EvaluacionReqMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");

            Property(x => x.idDiv).HasColumnName("idDiv");
            Property(x => x.texto).HasColumnName("texto");
            Property(x => x.inputFile).HasColumnName("inputFile");
            Property(x => x.lblInput).HasColumnName("lblInput");
            
            Property(x => x.tipoFile).HasColumnName("tipoFile");
            Property(x => x.txtAComentario).HasColumnName("txtAComentario");
            Property(x => x.txtPlaneacion).HasColumnName("txtPlaneacion");
            Property(x => x.txtResponsable).HasColumnName("txtResponsable");
            Property(x => x.txtFechaCompromiso).HasColumnName("txtFechaCompromiso"); 

            ToTable("tblCO_ADP_EvaluacionReq");
        }
    }
}
