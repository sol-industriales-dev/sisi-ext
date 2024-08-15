using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Maquinaria.Catalogo
{
    class MaquinariaMapping : EntityTypeConfiguration<tblM_CatMaquina>
    {
        public MaquinariaMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.noEconomico).HasColumnName("noEconomico");
            Property(x => x.descripcion).HasColumnName("descripcion");
            Property(x => x.grupoMaquinariaID).HasColumnName("grupoMaquinariaID");
            Property(x => x.modeloEquipoID).HasColumnName("modeloEquipoID");
            Property(x => x.marcaID).HasColumnName("marcaID");
            Property(x => x.anio).HasColumnName("anio");
            Property(x => x.placas).HasColumnName("placas");
            Property(x => x.noSerie).HasColumnName("noSerie");
            Property(x => x.aseguradoraID).HasColumnName("aseguradoraID");
            Property(x => x.noPoliza).HasColumnName("noPoliza");
            Property(x => x.TipoCombustibleID).HasColumnName("tipoCombustibleID");
            Property(x => x.capacidadTanque).HasColumnName("capacidadTanque");
            Property(x => x.unidadCarga).HasColumnName("unidadCarga");
            Property(x => x.capacidadCarga).HasColumnName("capacidadCarga");
            Property(x => x.horometroAdquisicion).HasColumnName("horometroAdquisicion");
            Property(x => x.horometroActual).HasColumnName("horometroActual");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.renta).HasColumnName("renta");
            Property(x => x.fechaAdquisicion).HasColumnName("fechaAdquisicion");
            Property(x => x.fechaPoliza).HasColumnName("fechaPoliza");
            Property(x => x.tipoEncierro).HasColumnName("tipoEncierro");
            Property(x => x.proveedor).HasColumnName("proveedor");
            Property(x => x.centro_costos).HasColumnName("centro_costos");
            Property(x => x.ComentarioStandBy).HasColumnName("ComentarioStandBy");
            Property(x => x.TipoCaptura).HasColumnName("TipoCaptura");
            Property(x => x.ProveedorID).HasColumnName("ProveedorID");
            HasRequired(x => x.aseguradora).WithMany().HasForeignKey(y => y.aseguradoraID);
            HasRequired(x => x.grupoMaquinaria).WithMany().HasForeignKey(y => y.grupoMaquinariaID);
            HasRequired(x => x.marca).WithMany().HasForeignKey(y => y.marcaID);
            HasRequired(x => x.modeloEquipo).WithMany().HasForeignKey(y => y.modeloEquipoID);


            Property(x => x.fechaEntregaSitio).HasColumnName("fechaEntregaSitio");
            Property(x => x.lugarEntregaProveedor).HasColumnName("lugarEntregaProveedor");
            Property(x => x.ordenCompra).HasColumnName("ordenCompra");
            Property(x => x.costoEquipo).HasColumnName("costoEquipo");
            Property(x => x.numArreglo).HasColumnName("numArreglo");
            Property(x => x.marcaMotor).HasColumnName("marcaMotor");
            Property(x => x.modeloMotor).HasColumnName("modeloMotor");
            Property(x => x.numSerieMotor).HasColumnName("numSerieMotor");
            Property(x => x.arregloCPL).HasColumnName("arregloCPL");


            Property(x => x.CondicionUso).HasColumnName("CondicionUso");
            Property(x => x.tipoAdquisicion).HasColumnName("tipoAdquisicion");
            Property(x => x.fabricacion).HasColumnName("fabricacion");
            Property(x => x.numPedimento).HasColumnName("numPedimento");


            Property(x => x.CostoRenta).HasColumnName("CostoRenta");
            Property(x => x.UtilizacionHoras).HasColumnName("UtilizacionHoras");
            Property(x => x.TipoCambio).HasColumnName("TipoCambio");

            Property(x => x.TipoBajaID).HasColumnName("TipoBajaID");
            Property(x => x.IdUsuarioBaja).HasColumnName("IdUsuarioBaja");
            Property(x => x.fechaBaja).HasColumnName("fechaBaja");
            Property(x => x.LibreAbordo).HasColumnName("LibreAbordo");
            Property(x => x.kmBaja).HasColumnName("kmBaja");
            Property(x => x.HorometroBaja).HasColumnName("HorometroBaja");


            Property(x => x.Comentario).HasColumnName("Comentario");
            Property(x => x.Garantia).HasColumnName("Garantia");

            Property(x => x.DepreciacionCapturada).HasColumnName("DepreciacionCapturada");
            Property(x => x.redireccionamientoVenta).HasColumnName("redireccionamientoVenta");
            Property(x => x.empresa).HasColumnName("empresa");
            Property(x => x.tieneSeguro).HasColumnName("tieneSeguro");
            Property(x => x.ManualesOperacion).HasColumnName("ManualesOperacion");

            ToTable("tblM_CatMaquina");
        }
    }
}
