using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Multiempresa;

namespace Data.EntityFramework.Mapping.Maquinaria.Catalogo
{
    public class ComponenteMapping : EntityTypeConfiguration<tblM_CatComponente>
    {
        ComponenteMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.noComponente).HasColumnName("noComponente");
            //Property(x => x.descripcion).HasColumnName("descripcion");
            //Property(x => x.noSerie).HasColumnName("noSerie");
            Property(x => x.numParte).HasColumnName("numParte");
            Property(x => x.costo).HasColumnName("costo");
            Property(x => x.cicloVidaHoras).HasColumnName("cicloVidaHoras");
            Property(x => x.horaCicloActual).HasColumnName("horaCicloActual");
            Property(x => x.horasAcumuladas).HasColumnName("horasAcumuladas");
            Property(x => x.subConjuntoID).HasColumnName("subConjuntoID");
            Property(x => x.marcaComponenteID).HasColumnName("marcaComponenteID");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.conjuntoID).HasColumnName("conjuntoID");
            Property(x => x.posicionID).HasColumnName("posicionID");
            Property(x => x.nombre_Corto).HasColumnName("nombre_Corto");
            Property(x => x.trackComponenteID).HasColumnName("trackComponenteID");
            Property(x => x.fecha).HasColumnName("fecha");
            Property(x => x.modeloEquipoID).HasColumnName("modeloEquipoID");
            Property(x => x.grupoID).HasColumnName("grupoID");
            Property(x => x.centroCostos).HasColumnName("centroCostos");
            Property(x => x.proveedorID).HasColumnName("proveedorID");
            Property(x => x.garantia).HasColumnName("garantia");
            Property(x => x.falla).HasColumnName("falla");
            Property(x => x.vidaInicio).HasColumnName("vidaInicio");
            Property(x => x.intercambio).HasColumnName("intercambio");
            Property(x => x.ordenCompra).HasColumnName("ordenCompra");

            Property(x => x.horasCicloInicio).HasColumnName("horasCicloInicio");
            Property(x => x.horasAcumuladasInicio).HasColumnName("horasAcumuladasInicio");

            //HasRequired(x => x.modeloEquipo).WithMany().HasForeignKey(y => y.modeloEquipoID);
            HasRequired(x => x.conjunto).WithMany().HasForeignKey(y => y.conjuntoID);

            // HasRequired(x => x.marcaComponente).WithMany().HasForeignKey(y => y.marcaComponenteID);
            HasRequired(x => x.subConjunto).WithMany().HasForeignKey(y => y.subConjuntoID);
            //HasRequired(x => x.Maquinaria_Componente).WithMany().HasForeignKey(y => y.trackComponenteID);

            //HasRequired(x => x.track).WithMany().HasForeignKey(y => y.trackComponenteID);
            HasRequired(x => x.proveedor).WithMany().HasForeignKey(y => y.proveedorID);
            ToTable("tblM_CatComponente");
        }
    }
}