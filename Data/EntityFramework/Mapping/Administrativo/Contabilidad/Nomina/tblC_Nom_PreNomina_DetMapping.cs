using Core.Entity.Administrativo.Contabilidad.Nomina;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_PreNomina_DetMapping : EntityTypeConfiguration<tblC_Nom_PreNomina_Det>
    {
        public tblC_Nom_PreNomina_DetMapping()
        {
            HasKey(x => x.id);
            Property(x => x.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnName("id");
            Property(x => x.prenominaID).HasColumnName("prenominaID");
            Property(x => x.orden).HasColumnName("orden");
            Property(x => x.empleadoCve).HasColumnName("empleadoCve");
            Property(x => x.empleadoNombre).HasColumnName("empleadoNombre");
            Property(x => x.puesto).HasColumnName("puesto");
            Property(x => x.sueldoSemanal).HasColumnName("sueldoSemanal");
            Property(x => x.dias).HasColumnName("dias");
            Property(x => x.nominaBase).HasColumnName("nominaBase");
            Property(x => x.descuento).HasColumnName("descuento");
            Property(x => x.prestamo).HasColumnName("prestamo");
            Property(x => x.axa).HasColumnName("axa");
            Property(x => x.descuentoFamsa).HasColumnName("descuentoFamsa");
            Property(x => x.pensionAlimenticia).HasColumnName("pensionAlimenticia");
            Property(x => x.fonacot).HasColumnName("fonacot");
            Property(x => x.infonavit).HasColumnName("infonavit");
            Property(x => x.sindicato).HasColumnName("sindicato");
            Property(x => x.fondoAhorroNomina).HasColumnName("fondoAhorroNomina");
            Property(x => x.totalNomina).HasColumnName("totalNomina");
            Property(x => x.complementoNomina).HasColumnName("complementoNomina");
            Property(x => x.fondoAhorroComplemento).HasColumnName("fondoAhorroComplemento");
            Property(x => x.bonoZona).HasColumnName("bonoZona");
            Property(x => x.bonoProduccion).HasColumnName("bonoProduccion");
            Property(x => x.otros).HasColumnName("otros");
            Property(x => x.primaVacacional).HasColumnName("primaVacacional");
            Property(x => x.primaDominical).HasColumnName("primaDominical");
            Property(x => x.hrExtra).HasColumnName("hrExtra");
            Property(x => x.hrExtraValor).HasColumnName("hrExtraValor");
            Property(x => x.importeExtra).HasColumnName("importeExtra");
            Property(x => x.diaHrExtra).HasColumnName("diaHrExtra");
            Property(x => x.diaExtraValor).HasColumnName("diaExtraValor");
            Property(x => x.importeDiaExtra).HasColumnName("importeDiaExtra");
            Property(x => x.totalComplemento).HasColumnName("totalComplemento");
            Property(x => x.totalPagar).HasColumnName("totalPagar");
            Property(x => x.porcentajeTotalPagar).HasColumnName("porcentajeTotalPagar");
            Property(x => x.totalRealPagar).HasColumnName("totalRealPagar");
            Property(x => x.valesDespensa).HasColumnName("valesDespensa");
            Property(x => x.totalDeposito).HasColumnName("totalDeposito");
            Property(x => x.cuenta).HasColumnName("cuenta");
            Property(x => x.clabeInterbancaria).HasColumnName("clabeInterbancaria");
            Property(x => x.banco).HasColumnName("banco");
            Property(x => x.observaciones).HasColumnName("observaciones");
            Property(x => x.estatus).HasColumnName("estatus");
            Property(x => x.apoyoColectivo).HasColumnName("apoyoColectivo");
            Property(x => x.onp).HasColumnName("onp");
            Property(x => x.afp).HasColumnName("afp");
            Property(x => x.afpSeguros).HasColumnName("afpSeguros");
            Property(x => x.afpComision).HasColumnName("afpComision");
            Property(x => x.sta).HasColumnName("sta");
            Property(x => x.adelantoQuincena).HasColumnName("adelantoQuincena");
            Property(x => x.adelantoGratSemestre).HasColumnName("adelantoGratSemestre");

            Property(x => x.diaFestivo).HasColumnName("diaFestivo");
            Property(x => x.diaFestivoValor).HasColumnName("diaFestivoValor");
            Property(x => x.importeDiaFestivo).HasColumnName("importeDiaFestivo");

            Property(x => x.transporte).HasColumnName("transporte");
            Property(x => x.comisiones).HasColumnName("comisiones");
            Property(x => x.retencion).HasColumnName("retencion");
            Property(x => x.fsp).HasColumnName("fsp");
            Property(x => x.bonoCuadrado).HasColumnName("bonoCuadrado");
            Property(x => x.bonoCuadradoDiario).HasColumnName("bonoCuadradoDiario");

            //HasRequired(x => x.prenomina).WithMany().HasForeignKey(y => y.prenominaID);

            ToTable("tblC_Nom_PreNomina_Det");
        }
    }
}
