using Core.Entity.ControlObra.GestionDeCambio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.ControlObra.Gestion
{
    public class ordenesDeCambioDTO
    {

        public int id { get; set; }
        public DateTime fechaEfectiva { get; set; }
        public string Proyecto { get; set; }
        public string CLiente { get; set; }
        public string Contratista { get; set; }
        public string Direccion { get; set; }
        public string NoOrden { get; set; }
        public bool esCobrable { get; set; }
        public string cc { get; set; }
        public string Antecedentes { get; set; }
        public List<tblCO_OC_Montos> lstMontos { get; set; }
        public SoportesEvidenciaDTO lstSoportesEvidencia { get; set; }
        public List<tblCO_OC_Firmas> lstFirmas { get; set; }
        public int idSubContratista { get; set; }
        public int status { get; set; }
        public bool voboPMO { get; set; }
        public string tabla { get; set; }
        public string numeroDeContrato { get; set; }
        public string ubicacionProyecto { get; set; }
        public decimal totalDeMontos { get; set; }
        public int idContrato { get; set; }
        public string noContrato { get; set; }
        public string alcancesNuevosDescripcion { get; set; }
        public string modificacionesPorCambioDescripcion { get; set; }
        public string requerimientosDeCampoDescripcion { get; set; }
        public string ajusteDeVolumenesDescripcion { get; set; }
        public string serviciosYSuministrosDescripcion { get; set; }
        public string fechaDescripcion { get; set; }
        public DateTime fechaSuscripcion { get; set; }
        public DateTime fechaExpiracion { get; set; }

        public string otrasCondicioes { get; set; }
        public string nombreDelArchivo { get; set; }

        public int turno { get; set; }

        public HttpPostedFileBase AntecedentesArchivos { get; set; }
        public HttpPostedFileBase AlcancesNuevosArchivos { get; set; }
        public HttpPostedFileBase modificacionArchvios { get; set; }
        public HttpPostedFileBase requerimientosArchivos { get; set; }
        public HttpPostedFileBase ajusteDeVolumenesArchivos { get; set; }
        public HttpPostedFileBase serviciosYSuministrosArchivos { get; set; }
        public List<cveEmpleadosDTO> cveEmpleados { get; set; }
        public List<string> nombreEmpleados { get; set; }

        public int tipo { get; set; }
        public string representanteLegal { get; set; }
        public List<AutorizanteDTO> listaAutorizantes { get; set; }
        public decimal montoContractual { get; set; }
        public DateTime? fechaInicial { get; set; }
        public DateTime? fechaFinal { get; set; }
        public DateTime? fechaAmpliacion { get; set; }
        public DateTime? fechaAmpliacionAcumulada { get; set; }
        public string filtroCC { get; set; }

        public string estado { get; set; }
        public string municipio { get; set; }
        public string subcontratistaNombre { get; set; }
        public bool esValidada { get; set; }
        public bool archivoValidado { get; set; }
        public bool editar { get; set; }
        public bool archivoCargado { get; set; }
    }
}
