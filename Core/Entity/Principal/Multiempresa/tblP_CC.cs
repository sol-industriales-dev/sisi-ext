using Core.Entity.Administrativo.FacultamientosDpto;
using Core.Entity.Principal.Usuarios;
using Core.Entity.ControlObra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace Core.Entity.Principal.Multiempresa
{
    public class tblP_CC
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public string areaCuenta { get; set; }
        public string descripcion { get; set; }
        public string ccRH { get; set; }
        public string descripcionRH { get; set; }
        public bool estatus { get; set; }
        public int? departamentoID { get; set; }
        public int? grupoID { get; set; }
        public string abreviacion { get; set; }
        [ForeignKey("departamentoID")]
        public virtual tblP_Departamento departamento { get; set; }
        [ForeignKey("grupoID")]
        public virtual tblFA_Grupos grupo { get; set; }
        public virtual List<tblFA_Paquete> paquetes { get; set; }
        public virtual List<tblCO_Capitulos> capitulo { get; set; }
        public bool esQuincenaNormal { get; set; }
        public bool isBajio { get; set; }
        public DateTime? fechaArranque { get; set; }
        public int ordernFlujoEfectivo { get; set; }
        public string st_ppto { get; set; }
        public string valida_anio { get; set; }
    }
}
