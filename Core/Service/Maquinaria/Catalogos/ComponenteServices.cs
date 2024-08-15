using Core.DAO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Overhaul;
using Core.Entity.Principal.Multiempresa;
using Core.DTO.Principal.Generales;
using Core.DTO.Maquinaria.Overhaul;

namespace Core.Service.Maquinaria.Catalogos
{
    public class ComponenteServices : IComponenteDAO
    {
        #region Atributos
        private IComponenteDAO m_componenteDAO;
        #endregion
        #region Propiedades
        public IComponenteDAO ComponenteDAO
        {
            get { return m_componenteDAO; }
            set { m_componenteDAO = value; }
        }
        #endregion
        #region Constructores
        public ComponenteServices(IComponenteDAO componenteDAO)
        {
            this.ComponenteDAO = componenteDAO;
        }
        #endregion

        public void Guardar(tblM_CatComponente obj)
        {
            ComponenteDAO.Guardar(obj);
        }
        public List<ComponenteDTO> FillGrid_Componente(ComponenteDTO obj)
        {
            return ComponenteDAO.FillGrid_Componente(obj);
        }
        public List<tblM_CatConjunto> FillCboConjuntos(bool estatus)
        {
            return ComponenteDAO.FillCboConjuntos(estatus);
        }
        public List<tblM_CatSubConjunto> FillCboSubConjuntos(int idConjunto)
        {
            return ComponenteDAO.FillCboSubConjuntos(idConjunto);
        }
        public List<ComboDTO> FillCboGrupoMaquinaria()
        {
            return ComponenteDAO.FillCboGrupoMaquinaria();
        }
        public List<tblM_CatModeloEquipo> FillCboModeloEquipo(int idGrupo)
        {
            return ComponenteDAO.FillCboModeloEquipo(idGrupo);
        }
        public List<ComboDTO> FillCboConjunto(int idModelo)
        {
            return ComponenteDAO.FillCboConjunto(idModelo);
        }
        //public List<int> getSubConjuntoPosiciones(int subConjunto)
        //{
        //    return ComponenteDAO.getSubConjuntoPosiciones(subConjunto);
        //}

        public List<tblM_CatModeloEquipo> FillCboFiltroModeloEquipo()
        {
            return ComponenteDAO.FillCboFiltroModeloEquipo();
        }
        public List<cboPrefijoModeloDTO> FillCboPrefijoModelo(int idModelo)
        {
            return ComponenteDAO.FillCboPrefijoModelo(idModelo);
        }
        public List<tblM_CatLocacionesComponentes> FillCboLocaciones(int tipoBusqueda)
        {
            return ComponenteDAO.FillCboLocaciones(tipoBusqueda);
        }
        public List<tblM_CatMaquina> FillCboLocacionesMaquina(int idModelo)
        {
            return ComponenteDAO.FillCboLocacionesMaquina(idModelo);
        }
        public int GuardarTrackingComponente(tblM_CatComponente obj, int locacion, DateTime fecha, int tipoLocacion, bool reciclado, string ordenCompra, string costo) 
        {
            return ComponenteDAO.GuardarTrackingComponente(obj, locacion, fecha, tipoLocacion, reciclado, ordenCompra, costo);
        }

        public void DeleteComponente(int idComponente) {
            ComponenteDAO.DeleteComponente(idComponente);
        }

        public List<tblP_CC> FillCboCentroCostros() {
            return ComponenteDAO.FillCboCentroCostros();
        }

        public List<tblM_CatSubConjunto> FillCboSubConjuntos(List<int> idConjunto, int idModelo) 
        {
            return ComponenteDAO.FillCboSubConjuntos(idConjunto, idModelo);
        }
        public tblM_trackComponentes getLocacion(int idComponente) 
        {
            return ComponenteDAO.getLocacion(idComponente);
        }

        public List<tblM_CatMarcasComponentes> getMarcas() 
        {
            return ComponenteDAO.getMarcas();
        }
        public tblM_CatMarcasComponentes getMarcaComponenteByID(int id)
        {
            return ComponenteDAO.getMarcaComponenteByID(id);
        }

        public string getNumParte(int idModelo, int idSubconjunto)
        {
            return ComponenteDAO.getNumParte(idModelo, idSubconjunto);
        }
        public tblM_CatComponente getComponenteByID(int idComponente)
        {
            return ComponenteDAO.getComponenteByID(idComponente);
        }
        public string getLocacionDescripcion(int idComponente)
        {
            return ComponenteDAO.getLocacionDescripcion(idComponente);
        }
        public string getCCByID(int id)
        {
            return ComponenteDAO.getCCByID(id);
        }
        public void guardarModificaciones(int cicloVidaHoras, int garantia, int estatusNuevo, List<ComboDTO> cc, string descripcionComponente, string locacion, int subconjunto, bool estatusActual, int modelo) 
        {
            ComponenteDAO.guardarModificaciones(cicloVidaHoras, garantia, estatusNuevo, cc, descripcionComponente, locacion, subconjunto, estatusActual, modelo);
        }
        public List<tblM_CatComponente> getComponentesByIDs(List<int> arrComponentes)
        {
            return ComponenteDAO.getComponentesByIDs(arrComponentes);
        }
        public List<ComboDTO> FillCboPosicionesComponente(int idSubconjunto)
        {
            return ComponenteDAO.FillCboPosicionesComponente(idSubconjunto);
        }
        public bool ActualizarTracking(int idComponente, int idTracking) 
        {
            return ComponenteDAO.ActualizarTracking(idComponente, idTracking);
        }
        public List<int> GetMaquinaByListaCC(List<string> obras)
        {
            return ComponenteDAO.GetMaquinaByListaCC(obras);
        }
    }
}
