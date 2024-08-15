using Core.DAO.Maquinaria.Inventario.ControlCalidad;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario.ControlCalidad
{
    public class ControlCalidadService : IControlCalidadDAO
    {
        #region Atributos
        private IControlCalidadDAO m_ControlCalidadDAO;
        #endregion
        #region Propiedades
        public IControlCalidadDAO ControlCalidadDAO
        {
            get { return m_ControlCalidadDAO; }
            set { m_ControlCalidadDAO = value; }
        }
        #endregion
        #region Constructores
        public ControlCalidadService(IControlCalidadDAO controlCalidadDAO)
        {
            this.ControlCalidadDAO = controlCalidadDAO;
        }
        #endregion

        public List<tblM_CatControlCalidad> getListControlCalidad()
        {
            return ControlCalidadDAO.getListControlCalidad();
        }

        public tblM_CatControlCalidad getControlCalidadById(int idAsignacion, int TipoControl)
        {
            return ControlCalidadDAO.getControlCalidadById(idAsignacion, TipoControl);
        }
        public tblM_CatControlCalidad getControlCalidadById(int idAsignacion, int TipoControl, int TipoFiltro)//prueba
        {
            return ControlCalidadDAO.getControlCalidadById(idAsignacion, TipoControl, TipoFiltro);
        }

        public tblM_CatControlCalidad saveControlCalidad(tblM_CatControlCalidad obj)
        {
            return ControlCalidadDAO.saveControlCalidad(obj);
        }

        public tblM_CatControlCalidad getByIDAsignacion(int id)
        {
            return ControlCalidadDAO.getByIDAsignacion(id);
        }

        public tblM_CatControlCalidad getByIDAsignacionTipo(int id, int tipoControl)
        {
            return ControlCalidadDAO.getByIDAsignacionTipo(id, tipoControl);
        }

        public Dictionary<string, object> guardarControlMovimientoMaquinaria(tblM_CatControlCalidad objCalidad,
                                                     List<tblM_RelPreguntaControlCalidad> lstRespuestas,
                                                     tblM_ControlMovimientoMaquinaria objControl,
                                                     string areaCuentaDestino, string areaCuentaOrigen,
                                                     int tipoControl, int envioEspecial)
        {
            return ControlCalidadDAO.guardarControlMovimientoMaquinaria(objCalidad, lstRespuestas, objControl, areaCuentaDestino, areaCuentaOrigen, tipoControl, envioEspecial);
        }


    }
}
