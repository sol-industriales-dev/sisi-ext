using Core.DAO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario
{
    public class AutorizacionSolicitudesServices : IAutorizacionSolicitudesDAO
    {

        #region Atributos
        private IAutorizacionSolicitudesDAO m_AutorizacionSolicitudesDAO;
        #endregion
        #region Propiedades
        public IAutorizacionSolicitudesDAO AutorizacionSolicitudesDAO
        {
            get { return m_AutorizacionSolicitudesDAO; }
            set { m_AutorizacionSolicitudesDAO = value; }
        }
        #endregion
        #region Constructores
        public AutorizacionSolicitudesServices(IAutorizacionSolicitudesDAO autorizacionSolicitudesDAO)
        {
            this.AutorizacionSolicitudesDAO = autorizacionSolicitudesDAO;
        }
        #endregion

        public void Guardar(tblM_AutorizacionSolicitudes obj)
        {
            AutorizacionSolicitudesDAO.Guardar(obj);
        }
        public tblM_AutorizacionSolicitudes getAutorizadores(int idSolicitud)
        {
            return AutorizacionSolicitudesDAO.getAutorizadores(idSolicitud);
        }

        public List<tblM_CatMaquina> ListaEconomicos(int grupoid, int modeloid, int tipoId,int idEconomico)
        {
            return AutorizacionSolicitudesDAO.ListaEconomicos(grupoid, modeloid, tipoId, idEconomico);
        }

        public tblM_AutorizacionSolicitudes GetAutorizacionSolicitudes(int id)
        {
            return AutorizacionSolicitudesDAO.GetAutorizacionSolicitudes(id);
        }

     
    }
}
