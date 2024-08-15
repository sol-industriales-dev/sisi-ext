using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class ConceptoDTO
    {
        public int id { get; set; }
        public int anio { get; set; }
        public int idCC { get; set; }
        public int idAgrupacion { get; set; }
        public int idConcepto { get; set; }
        public string concepto { get; set; }
        public int insumo { get; set; }
        public string insumoDescripcion { get; set; }
        public decimal cantPpto { get; set; }
        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string cuentaDescripcion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public int year { get; set; }
        public int mes { get; set; }
        public List<int> arrConstruplan { get; set; }
        public List<int> arrArrendadora { get; set; }
        public int empresa { get; set; }
        public int idMes { get; set; }
        public string nombreMes { get; set; }
        public List<int> arrCapturasID { get; set; }
        public bool costosAdministrativos { get; set; }
    }
}
