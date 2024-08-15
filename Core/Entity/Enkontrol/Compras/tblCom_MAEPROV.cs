using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Enkontrol.Compras
{
    public class tblCom_MAEPROV
    {
        public int id { get; set; }
        public string PRVCCODIGO { get; set; }
        public string PRVCNOMBRE { get; set; }
        public string PRVCDIRECC { get; set; }
        public string PRVCLOCALI { get; set; }
        public string PRVCPAISAC { get; set; }
        public string PRVCTELEF1 { get; set; }
        public string PRVCFAXACR { get; set; }
        public string PRVCTIPOAC { get; set; }
        public string PRVCGIROAC { get; set; }
        public string PRVCREPRES { get; set; }
        public string PRVCCARREP { get; set; }
        public string PRVCTELREP { get; set; }
        public DateTime? PRVDFECCRE { get; set; }
        public string PRVCUSER { get; set; }
        public string PRPVCRUC { get; set; }
        public string PRVCRUC { get; set; }
        public string PRVCABREVI { get; set; }
        public string PRVCESTADO { get; set; }
        public DateTime? PRVDFECMOD { get; set; }
        public string PRVEMAIL { get; set; }
        public string PRVCODFAB { get; set; }
        public string PRVPAGO { get; set; }
        public string PRVFACTOR { get; set; }
        public string PRVGLOSA { get; set; }
        public string PRVREPRESENTANTE { get; set; }
        public string PRVCONTACTO { get; set; }
        public string PRVTELREP { get; set; }
        public string PRVFAXREP { get; set; }
        public string PRVEMAILREP { get; set; }
        public string PRVCTIPO_DOCUMENTO { get; set; }
        public string PRVCAPELLIDO_PATERNO { get; set; }
        public string PRVCAPELLIDO_MATERNO { get; set; }
        public string PRVCPRIMER_NOMBRE { get; set; }
        public string PRVCSEGUNDO_NOMBRE { get; set; }
        public string PRVCDOCIDEN { get; set; }
        public DateTime? FEC_INACTIVO_BLOQUEADO { get; set; }
        public string COD_AUDITORIA { get; set; }
        public string UBIGEO { get; set; }
        public bool FLGPORTAL_PROVEEDOR { get; set; }
        public int? id_usuarioCreacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public int? id_usuarioModificacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public int? id_usuarioVobo { get; set; }
        public DateTime? fechaVobo{ get; set; }
        public int? id_usuarioAutorizo { get; set; }
        public DateTime? fechaAutorizo { get; set; }
        public bool statusAutorizacion { get; set; }
        public bool Vobo { get; set; }
        public bool Autorizado { get; set; }
        public bool statusNotificacion { get; set; }

    }
}
