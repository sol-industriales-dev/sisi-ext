using Core.Entity.RecursosHumanos.ActoCondicion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.RecursosHumanos.ActoCondicion
{
    public class ActoMapping : EntityTypeConfiguration<tblRH_AC_Acto>
    {
        public ActoMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.claveEmpleado).HasColumnName("claveEmpleado");
            Property(x => x.nombre).HasColumnName("nombre");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.fechaIngreso).HasColumnName("fechaIngreso");
            Property(x => x.accionID).HasColumnName("accionID");
            HasRequired(x => x.accion).WithMany().HasForeignKey(x => x.accionID);
            Property(x => x.tipoActo).HasColumnName("tipoActo");
            Property(x => x.esExterno).HasColumnName("esExterno");
            Property(x => x.claveContratista).HasColumnName("claveContratista");
            Property(x => x.claveInformo).HasColumnName("claveInformo");
            Property(x => x.nombreInformo).HasColumnName("nombreInformo");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.cc).HasColumnName("cc");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.clasificacionID).HasColumnName("clasificacionID");
            HasRequired(x => x.clasificacion).WithMany().HasForeignKey(x => x.clasificacionID);
            Property(x => x.procedimientoID).HasColumnName("procedimientoID");
            //HasRequired(x => x.procedimientoViolado).WithMany().HasForeignKey(x => x.procedimientoID); // OMAR
            Property(x => x.fechaSuceso).HasColumnName("fechaSuceso");
            Property(x => x.claveSupervisor).HasColumnName("claveSupervisor");
            Property(x => x.nombreSupervisor).HasColumnName("nombreSupervisor");
            Property(x => x.departamentoID).HasColumnName("departamentoID");
            HasRequired(x => x.departamento).WithMany().HasForeignKey(x => x.departamentoID);
            Property(x => x.subclasificacionDepID).HasColumnName("subclasificacionDepID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.fechaProcesoCompleto).HasColumnName("fechaProcesoCompleto");
            Property(x => x.numeroInfraccion).HasColumnName("numeroInfraccion");
            Property(x => x.nivelInfraccion).HasColumnName("nivelInfraccion");
            Property(x => x.nivelInfraccionAcumulado).HasColumnName("nivelInfraccionAcumulado");
            Property(x => x.numeroFalta).HasColumnName("numeroFalta");
            Property(x => x.compromiso).HasColumnName("compromiso");
            Property(x => x.rutaEvidencia).HasColumnName("rutaEvidencia");
            Property(x => x.activo).HasColumnName("activo");
            Property(x => x.usuarioCreadorID).HasColumnName("usuarioCreadorID");
            HasRequired(x => x.usuarioCreador).WithMany().HasForeignKey(x => x.usuarioCreadorID);
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.idAgrupacion).HasColumnName("idAgrupacion");
            //HasOptional(x => x.agrupacion).WithMany().HasForeignKey(x => x.idAgrupacion); // OMAR
            Property(x => x.firmaEmpleado).HasColumnName("firmaEmpleado");
            Property(x => x.firmaSupervisor).HasColumnName("firmaSupervisor");
            Property(x => x.firmaSST).HasColumnName("firmaSST");
            Property(x => x.claveSST).HasColumnName("claveSST");
            Property(x => x.nombreSST).HasColumnName("nombreSST");
            Property(x => x.fechaFirmado).HasColumnName("fechaFirmado");
            Property(x => x.rutaActa).HasColumnName("rutaActa");
            Property(x => x.fechaCargaActa).HasColumnName("fechaCargaActa");
            Property(x => x.clasificacionGeneralID).HasColumnName("clasificacionGeneralID");
            HasOptional(x => x.clasificacionGeneral).WithMany().HasForeignKey(x => x.clasificacionGeneralID);
            Property(x => x.cargaMasiva).HasColumnName("cargaMasiva");

            ToTable("tblRH_AC_Acto");
        }
    }
}
