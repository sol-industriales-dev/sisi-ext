using Core.DAO.Maquinaria.Inventario;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario
{
    public class ControlInternoMovimientosServices : IControlInternoMovimientosDAO
    {
        #region Atributos
        private IControlInternoMovimientosDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IControlInternoMovimientosDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public ControlInternoMovimientosServices(IControlInternoMovimientosDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion
        public void GuardarActualizar(tblM_ControMovimientoInterno obj)
        {
            interfazDAO.GuardarActualizar(obj);

        }

        public List<tblM_CatMaquina> FillCboEconomicos(string cc)
        {
            return interfazDAO.FillCboEconomicos(cc);
        }

        public tblM_CatMaquina GetDataEconomicoID(int id)
        {
            return interfazDAO.GetDataEconomicoID(id);
        }
        public string LoadFolio()
        {
            return interfazDAO.LoadFolio();
        }

        public List<tblM_ControMovimientoInterno> GetControlesRealizados(int filtro)
        {
            return interfazDAO.GetControlesRealizados(filtro);
        }

        public List<ComboDTO> FillCboEconomicosUsuarioID(int usuario)
        {
            return interfazDAO.FillCboEconomicosUsuarioID(usuario);
        }

        public List<ComboDTO> getCentrosCostos(int usuario)
        {
            return interfazDAO.getCentrosCostos(usuario);
        }
        public List<ComboDTO> getCentrosCostosRecepcion(string CentroCostos)
        {
            return interfazDAO.getCentrosCostosRecepcion(CentroCostos);
        }

        public int GetUsuarioAutoriza(string centroCostos)
        {
            return interfazDAO.GetUsuarioAutoriza(centroCostos);
        }
    }
}
