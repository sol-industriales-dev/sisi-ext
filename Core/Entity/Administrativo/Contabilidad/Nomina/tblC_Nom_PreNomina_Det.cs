using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_PreNomina_Det
    {
        public int id { get; set; }
        public int prenominaID { get; set; }
        //public virtual tblC_Nom_Prenomina prenomina { get; set; }
        public int orden { get; set; }
	    public int empleadoCve { get; set; }
	    public string empleadoNombre { get; set; }
	    public string puesto { get; set; }
	    public decimal sueldoSemanal { get; set; }
	    public decimal dias { get; set; }
	    public decimal nominaBase { get; set; }
        public decimal diasVacaciones { get; set; }
        public decimal nominaBaseVacaciones { get; set; }
	    public decimal descuento { get; set; }
        public decimal licenciaLuto { get; set; }
        public decimal prestamo { get; set; }
	    public decimal axa { get; set; }
	    public decimal descuentoFamsa { get; set; }
	    public decimal pensionAlimenticia { get; set; }
	    public decimal fonacot { get; set; }
	    public decimal infonavit { get; set; }
	    public decimal sindicato { get; set; }
	    public decimal fondoAhorroNomina { get; set; }
	    public decimal totalNomina { get; set; }
	    public decimal complementoNomina { get; set; }
	    public decimal fondoAhorroComplemento { get; set; }
	    public decimal bonoZona { get; set; }
	    public decimal bonoProduccion { get; set; }
	    public decimal otros { get; set; }
	    public decimal primaVacacional { get; set; }
	    public decimal primaDominical { get; set; }
	    public decimal hrExtra { get; set; }
	    public decimal hrExtraValor { get; set; }
	    public decimal importeExtra { get; set; }
	    public int diaHrExtra { get; set; }
	    public decimal diaExtraValor { get; set; }
	    public decimal importeDiaExtra { get; set; }
	    public decimal totalComplemento { get; set; }
	    public decimal totalPagar { get; set; }
        public decimal porcentajeTotalPagar { get; set; }
        public decimal totalRealPagar { get; set; }
        public decimal valesDespensa { get; set; }
        public decimal totalDeposito { get; set; }
	    public string cuenta { get; set; }
	    public string clabeInterbancaria { get; set; }
	    public string banco { get; set; }
        public string observaciones { get; set; }
        public bool estatus { get; set; }
        public decimal apoyoColectivo { get; set; }
        public int diaFestivo { get; set; }
        public decimal diaFestivoValor { get; set; }
        public decimal importeDiaFestivo { get; set; }

        //Peru
        public decimal onp { get; set; }
        public decimal afp { get; set; }
        public decimal afpSeguros { get; set; }
        public decimal afpComision { get; set; }
        public decimal sta { get; set; }
        public decimal adelantoQuincena { get; set; }
        public decimal adelantoGratSemestre { get; set; }
        public decimal asigFamiliar { get; set; }
        public decimal esSalud { get; set; }

        //Colombia
        public decimal transporte { get; set; }
        public decimal comisiones { get; set; }
        public decimal retencion { get; set; }
        public decimal fsp { get; set; }

        [NotMapped]
        public string cc { get; set; }
        //public string tipoContrato { get; set; }
        //public DateTime? fechaIngreso { get; set; }
        public decimal cesantia { get; set; }
        public decimal interesCesantia { get; set; }
        public decimal retroactivo { get; set; }
        public decimal prima { get; set; }
        public decimal vacaciones { get; set; }
        public decimal hrExtraDiurnasDominicales { get; set; }
        public decimal hrExtraDiurnasDominicalesValor { get; set; }
        public decimal importeExtraDiurnasDominicales { get; set; }
        [NotMapped]
        public virtual string fechaAntiguedad { get; set; }

        public decimal hrNocturnas { get; set; }
        public decimal hrNocturnasValor { get; set; }
        public decimal importeNocturnasExtra { get; set; }
        public decimal importeDiasVacaciones { get; set; }
        public int diasIncapacidad { get; set; }
        public decimal importeIncapacidad { get; set; }
        public decimal bonoRecreacion { get; set; }
        public decimal bonoCuadrado { get; set; }
        public decimal bonoCuadradoDiario { get; set; }
    }
}
