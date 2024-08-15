using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.SubContratistas;

namespace Core.DTO.Subcontratistas
{
    public class SubcontratistaDTO
    {
        public int id { get; set; }
        public int numeroProveedor { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string nombreCorto { get; set; }
        public string codigoPostal { get; set; }
        public string rfc { get; set; }
        public string correo { get; set; }
        public bool fisica { get; set; }
        public bool pendienteValidacion { get; set; }
        public bool tieneHistorial { get; set; }

        public List<tblX_RelacionSubContratistaDocumentacion> listaArchivos { get; set; }

        public int actaConstitutivaID { get; set; }
        public int poderRLID { get; set; }
        public int INEID { get; set; }
        public int cedulaFiscalID { get; set; }
        public int registroPatronalID { get; set; }
        public int objetoSocialVigenteID { get; set; }
        public int registroEspecializacionID { get; set; }
        public int comprobanteDomicilioID { get; set; }

        public string rutaActaConstitutiva { get; set; }
        public string rutaPoderRL { get; set; }
        public string rutaINE { get; set; }
        public string rutaCedulaFiscal { get; set; }
        public string rutaRegistroPatronal { get; set; }
        public string rutaObjetoSocialVigente { get; set; }
        public string rutaRegistroEspecializacion { get; set; }
        public string rutaComprobanteDomicilio { get; set; }

        public bool validadoActaConstitutiva { get; set; }
        public bool validadoPoderRL { get; set; }
        public bool validadoINE { get; set; }
        public bool validadoCedulaFiscal { get; set; }
        public bool validadoRegistroPatronal { get; set; }
        public bool validadoObjetoSocialVigente { get; set; }
        public bool validadoRegistroEspecializacion { get; set; }
        public bool validadoComprobanteDomicilio { get; set; }

        public bool aplicaActaConstitutiva { get; set; }
        public bool aplicaPoderRL { get; set; }
        public bool aplicaINE { get; set; }
        public bool aplicaCedulaFiscal { get; set; }
        public bool aplicaRegistroPatronal { get; set; }
        public bool aplicaObjetoSocialVigente { get; set; }
        public bool aplicaRegistroEspecializacion { get; set; }
        public bool aplicaComprobanteDomicilio { get; set; }
    }
}
