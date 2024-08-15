using Core.Entity.Enkontrol.Compras.Requisicion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Enkontrol.Compras.Requisicion
{
    class RequisicionMapping : EntityTypeConfiguration<tblCom_Req>
    {
        public RequisicionMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.numero).HasColumnName("numero");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.idLibreAbordo).HasColumnName("idLibreAbordo");
            Property(x => x.idTipoReqOc).HasColumnName("idTipoReqOc");
            Property(x => x.solicito).HasColumnName("solicito");
            Property(x => x.vobo).HasColumnName("vobo");
            Property(x => x.autorizo).HasColumnName("autorizo");
            Property(x => x.comentarios).HasColumnName("comentarios");
            Property(x => x.stEstatus).HasColumnName("stEstatus");
            Property(x => x.stImpresa).HasColumnName("stImpresa");
            Property(x => x.stAutoriza).HasColumnName("stAutoriza");
            Property(x => x.empAutoriza).HasColumnName("empAutoriza");
            Property(x => x.empModifica).HasColumnName("empModifica");
            Property(x => x.modifica).HasColumnName("modifica");
            Property(x => x.autoriza).HasColumnName("autoriza");
            Property(x => x.isTmc).HasColumnName("isTmc");
            Property(x => x.isActivos).HasColumnName("isActivos");
            Property(x => x.numVobo).HasColumnName("numVobo");
            Property(x => x.folioAsignado).HasColumnName("folioAsignado");
            Property(x => x.consigna).HasColumnName("consigna");
            Property(x => x.licitacion).HasColumnName("licitacion");
            Property(x => x.crc).HasColumnName("crc");
            Property(x => x.convenio).HasColumnName("convenio");
            Property(x => x.proveedor).HasColumnName("proveedor");
            Property(x => x.validadoAlmacen).HasColumnName("validadoAlmacen");
            Property(x => x.validadoCompras).HasColumnName("validadoCompras");
            Property(x => x.validadoRequisitor).HasColumnName("validadoRequisitor");
            Property(x => x.fechaValidacionAlmacen).HasColumnName("fechaValidacionAlmacen");
            Property(x => x.comprador).HasColumnName("comprador");
            Property(x => x.empleadoUltimaAccion).HasColumnName("empleadoUltimaAccion");
            Property(x => x.fechaUltimaAccion).HasColumnName("fechaUltimaAccion");
            Property(x => x.tipoUltimaAccion).HasColumnName("tipoUltimaAccion");
            Property(x => x.fechaSurtidoCompromiso).HasColumnName("fechaSurtidoCompromiso");
            Property(x => x.fechaEnvioCorreoProveedor).HasColumnName("fechaEnvioCorreoProveedor");
            Property(x => x.usuarioSolicita).HasColumnName("usuarioSolicita");
            Property(x => x.usuarioSolicitaUso).HasColumnName("usuarioSolicitaUso");
            Property(x => x.usuarioSolicitaEmpresa).HasColumnName("usuarioSolicitaEmpresa");
            Property(x => x.estatusRegistro).HasColumnName("estatusRegistro");

            ToTable("tblCom_Req");
        }
    }
}
