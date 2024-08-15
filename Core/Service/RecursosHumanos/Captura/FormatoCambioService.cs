using Core.DAO.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Maquinaria.Captura;

namespace Core.Service.RecursosHumanos.Captura
{
    public class FormatoCambioService : IFormatoCambio
    {

        #region Atributos
        private IFormatoCambio m_FormatoCambio;
        #endregion
        #region Propiedades
        public IFormatoCambio formatoCambio
        {
            get { return m_FormatoCambio; }
            set { m_FormatoCambio = value; }
        }
        #endregion
        #region Constructores
        #endregion
        public FormatoCambioService(IFormatoCambio FormatoCambio)
        {
            this.formatoCambio = FormatoCambio;
        }
        public tblRH_FormatoCambio getEmpleadoForId(int id, bool activo)
        {
            return formatoCambio.getEmpleadoForId(id,activo);
        }
        public List<tblRH_CatPuestos> getCatPuestos(string term)
        {
            return formatoCambio.getCatPuestos(term);
        }
        public int getFormatoCambioID(int id)
        {
            return formatoCambio.getFormatoCambioID(id);
        }

        public tblRH_FormatoCambio getFormatoByID(int id)
        {
            return formatoCambio.getFormatoByID(id);
        }
        
        public List<tblRH_CatTipoNomina> getCatTipoNomina()
        {
            return formatoCambio.getCatTipoNomina();
        }
        public List<tblRH_CatCentroCostos> getCC(string term)
        {
            return formatoCambio.getCC(term);
        }
        public List<tblRH_CatEmpleados> getCatEmpleados(string term)
        {
            return formatoCambio.getCatEmpleados(term);
        }
        public List<tblRH_CatEmpleados> getCatEmpleadosReclutamientos(string term)
        {
            return formatoCambio.getCatEmpleadosReclutamientos(term);
        }
        public List<tblRH_CatEmpleados> getCatEmpleadosGeneral(string term)
        {
            return formatoCambio.getCatEmpleadosGeneral(term);
        }
        public List<tblRH_CatRegistroPatronales> getCatRegistroPatronales(string term)
        {
            return formatoCambio.getCatRegistroPatronales(term);
        }
        public tblRH_FormatoCambio SaveChangesEmpleado(tblRH_FormatoCambio objEmpleado)
        {
            return formatoCambio.SaveChangesEmpleado(objEmpleado);
        }
        public List<tblRH_FormatoCambio> getListFormatosCambioPendientes(int id, string cc, int claveEmp, int estado, string tipo, int numero)
        {
            return formatoCambio.getListFormatosCambioPendientes(id, cc, claveEmp, estado, tipo, numero);
        }
        public void eliminarFormato(int formatoID)
        {
            formatoCambio.eliminarFormato(formatoID);
        }
        public List<tblRH_CatCentroCostos> getCCList()
        {
            return formatoCambio.getCCList();
        }
        public tblRH_FormatoCambio getFormatoCambioByID(int idFormatoCambio)
        {
            return formatoCambio.getFormatoCambioByID(idFormatoCambio);
        }
        public void EmpleadoEnkontrolToSigoplan()
        {
            formatoCambio.EmpleadoEnkontrolToSigoplan();
        }
        public bool getEmpleadoExclusion(int empleadoCVE)
        {
            return formatoCambio.getEmpleadoExclusion(empleadoCVE);
        }
        public bool validReporteExclusion(int id)
        {
            return formatoCambio.validReporteExclusion(id);
        }
        public List<tblRH_CatEmpleados> getCatEmpleadosTodos(string term)
        {
            return formatoCambio.getCatEmpleadosTodos(term);

        }
        public Dictionary<string, object> getDepartamentosCC(string cc)
        {
            return formatoCambio.getDepartamentosCC(cc);
        }
        public Dictionary<string, object> GetResponsableCC(string cc)
        {
            return formatoCambio.GetResponsableCC(cc);
        }
        public Dictionary<string, object> GetRegistroPatCC(string cc)
        {
            return formatoCambio.GetRegistroPatCC(cc);
        }

        public Dictionary<string, object> GetTabuladorByEmpleado(int puesto, int lineaNegocios, int categoria)
        {
            return formatoCambio.GetTabuladorByEmpleado(puesto, lineaNegocios, categoria);
        }

        #region PERMISOS
        public bool CheckEsEditarPuestos()
        {
            return formatoCambio.CheckEsEditarPuestos();
        }

        public bool GetPermisoSueldos()
        {
            return formatoCambio.GetPermisoSueldos();
        }
        #endregion

        public List<cboDTO> cboCentroCostos()
        {
            return formatoCambio.cboCentroCostos();
        }
    
    }
}
