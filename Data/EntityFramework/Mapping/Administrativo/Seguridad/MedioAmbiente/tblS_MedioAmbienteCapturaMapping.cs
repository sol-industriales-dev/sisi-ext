using Core.Entity.Administrativo.Seguridad.MedioAmbiente;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Seguridad.MedioAmbiente
{
    public class tblS_MedioAmbienteCapturaMapping : EntityTypeConfiguration<tblS_MedioAmbienteCaptura>
    {
        public tblS_MedioAmbienteCapturaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.folio).HasColumnName("folio");
            Property(x => x.consecutivo).HasColumnName("consecutivo");
            Property(x => x.tipoCaptura).HasColumnName("tipoCaptura");
            Property(x => x.idEmpresa).HasColumnName("idEmpresa");
            Property(x => x.idAgrupacion).HasColumnName("idAgrupacion");
            Property(x => x.idResponsableTecnico).HasColumnName("idResponsableTecnico");
            Property(x => x.fechaEntrada).HasColumnName("fechaEntrada");
            Property(x => x.cantidadContenedor).HasColumnName("cantidadContenedor");
            Property(x => x.tipoContenedor).HasColumnName("tipoContenedor");
            Property(x => x.plantaProcesoGeneracion).HasColumnName("plantaProcesoGeneracion");
            Property(x => x.tratamiento).HasColumnName("tratamiento");
            Property(x => x.manifiesto).HasColumnName("manifiesto");
            Property(x => x.fechaEmbarque).HasColumnName("fechaEmbarque");
            Property(x => x.tipoTransporte).HasColumnName("tipoTransporte");
            Property(x => x.idTransportistaTrayecto).HasColumnName("idTransportistaTrayecto");
            Property(x => x.fechaDestinoFinal).HasColumnName("fechaDestinoFinal");
            Property(x => x.idTransportistaDestinoFinal).HasColumnName("idTransportistaDestinoFinal");
            Property(x => x.estatusCaptura).HasColumnName("estatusCaptura");
            Property(x => x.idUsuarioCreacion).HasColumnName("idUsuarioCreacion");
            Property(x => x.idUsuarioModificacion).HasColumnName("idUsuarioModificacion");
            Property(x => x.fechaCreacion).HasColumnName("fechaCreacion");
            Property(x => x.fechaModificacion).HasColumnName("fechaModificacion");
            Property(x => x.esActivo).HasColumnName("esActivo");

            ToTable("tblS_MedioAmbienteCaptura");
        }
    }
}