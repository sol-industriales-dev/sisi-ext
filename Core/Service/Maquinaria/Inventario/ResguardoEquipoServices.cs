using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Multiempresa;

namespace Core.Service.Maquinaria.Inventario
{
    public class ResguardoEquipoServices : IResguardoEquipoDAO
    {

        #region Atributos
        private IResguardoEquipoDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private IResguardoEquipoDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public ResguardoEquipoServices(IResguardoEquipoDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores
        public List<tblM_CatPreguntaResguardoVehiculo> GetListaPreguntas()
        {
            return interfazDAO.GetListaPreguntas();
        }

        public List<tblRH_CatEmpleados> getCatEmpleados(string term, List<string> CentroCostos)
        {
            return interfazDAO.getCatEmpleados(term, CentroCostos);
        }

        public List<tblM_ResguardoVehiculosServicio> GetListaAutorizacionesPendientes(string cc, int obj)
        {
            return interfazDAO.GetListaAutorizacionesPendientes(cc, obj);
        }

        public string GetFechaVigenciaResguardo(int id)
        {
            return interfazDAO.GetFechaVigenciaResguardo(id);
        }

        public void GuardarResguardoVehiculos(tblM_ResguardoVehiculosServicio obj)
        {
            interfazDAO.GuardarResguardoVehiculos(obj);
        }


        public tblM_ResguardoVehiculosServicio getResguardoBYID(int obj)
        {
            return interfazDAO.getResguardoBYID(obj);
        }

        public tblRH_CatEmpleados getCatEmpleado(string id)
        {
            return interfazDAO.getCatEmpleado(id);
        }

        public List<int> GetEmpleadosResguardo()
        {
            return interfazDAO.GetEmpleadosResguardo();
        }

        public List<int> GetMaquinariaAsignada()
        {
            return interfazDAO.GetMaquinariaAsignada();
        }

        public List<tblM_ResguardoVehiculosServicio> getListaResguardosPendientesAutorizacion(string cc, int economicoID)
        {
            return interfazDAO.getListaResguardosPendientesAutorizacion(cc, economicoID);
        }

        public List<tblM_ResguardoVehiculosServicio> getListaResguardosPendientesLicencia(List<tblP_CC_Usuario> listObj)
        {
            return interfazDAO.getListaResguardosPendientesLicencia(listObj);
        }

        public List<tblM_ResguardoVehiculosServicio> getListaResguardosPendientesPoliza(List<tblP_CC_Usuario> listObj)
        {
            return interfazDAO.getListaResguardosPendientesPoliza(listObj);
        }


        public string getCCByArea(string area)
        {
            return interfazDAO.getCCByArea(area);
        }

        public string getMaquinaByID(int id) 
        {
            return interfazDAO.getMaquinaByID(id);
        }
        public string getNoEconomicoMaquinaByID(int id)
        {
            return interfazDAO.getNoEconomicoMaquinaByID(id);
        }
        public string getModeloByID(int id) 
        {
            return interfazDAO.getModeloByID(id);
        }
        public List<tblM_CatMaquina> getEquipoSinResguardo(string ac)
        {
            return interfazDAO.getEquipoSinResguardo(ac);
        }

        public List<dynamic> GetDocumentosResguardos()
        {
            return interfazDAO.GetDocumentosResguardos();
        }

        public List<tblM_ResguardoVehiculosServicio> GetCursosManejoVencidos()
        {
            return interfazDAO.GetCursosManejoVencidos();
        }

        public List<tblP_CC> GetCentrosCostos()
        {
            return interfazDAO.GetCentrosCostos();
        }

        public void NotificarCoordinadorSSOMA(string cc, int resguardoId, string economico)
        {
            interfazDAO.NotificarCoordinadorSSOMA(cc, resguardoId, economico);
        }

        public void QuitarNotificacionSSOMA(int resguardoId)
        {
            interfazDAO.QuitarNotificacionSSOMA(resguardoId);
        }
    }
}
