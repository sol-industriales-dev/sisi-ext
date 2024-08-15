using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Reclutamientos
{
    public class tblRH_REC_ExamenMedico
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int edad { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public int escolaridad { get; set; }
        public string otraEscolaridad { get; set; }
        public int estadoCivil { get; set; }
        public string otroEstadoCivil { get; set; }
        public int hijos { get; set; }
        public int puestoAnterior { get; set; }
        public string puestoAnteriorDesc { get; set; }
        public int puestoOcupar { get; set; }
        public string puestoOcuparDesc { get; set; }
        public bool alcoholismo { get; set; }
        public string observacionesAlcoholismo { get; set; }
        public bool tabaquismo { get; set; }
        public string observacionesTabaquismo { get; set; }
        public bool toxicomania { get; set; }
        public string observacionesToxicomania { get; set; }
        public bool lentes { get; set; }
        public string observacionesLentes { get; set; }
        public string tipoSanguineo { get; set; }
        public string visual { get; set; }
        public string auditiva { get; set; }
        public string TA { get; set; }
        public string pulso { get; set; }
        public string marchaPunta { get; set; }
        public string talon { get; set; }
        public string romberg { get; set; }
        public string arcosFlexion { get; set; }
        public string antecedentesFamiliares { get; set; }
        public string heredoFamiliar { get; set; }
        public string tratamiento { get; set; }
        public string rayosX { get; set; }
        public string menarca { get; set; }
        public string VSA { get; set; }
        public string numeroGestas { get; set; }
        public string ritmo { get; set; }
        public string MPF { get; set; }
        public string PIE { get; set; }
        public string OPI { get; set; }
        public string MET { get; set; }
        public string COC { get; set; }
        public string AMP { get; set; }
        public string THC { get; set; }
        public bool personaApta { get; set; }
        public string observacionesGenerales { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
    }
}
