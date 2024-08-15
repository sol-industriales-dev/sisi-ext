using Core.DAO.Administracion.Facultamiento;
using Core.DTO.Administracion.Facultamiento;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Facultamiento;
using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Administracion.Facultamiento
{
    public class FacultamientoServices : IFacultamientoDAO
    {
        #region Atributos
        private IFacultamientoDAO c_facultamiento;
        #endregion
        #region Propiedades
        public IFacultamientoDAO FacultamientoDAO
        {
            get { return c_facultamiento; }
            set { c_facultamiento = value; }
        }
        #endregion
        #region Constructores
        public FacultamientoServices(IFacultamientoDAO facultamientoDAO)
        {
            this.FacultamientoDAO = facultamientoDAO;
        }
        #endregion

        public List<tblFa_CatFacultamiento> getCCCompleto(List<string> cc, DateTime fechaI, DateTime fechaF)
        {
            return this.FacultamientoDAO.getCCCompleto(cc, fechaI, fechaF);

        }
        public tblFa_CatAuth saveFacultamiento(tblFa_CatFacultamiento obj, List<tblFa_CatAutorizacion> lstAut, List<tblFa_CatMonto> lstMonto, List<tblFa_CatAuth> lstAuth, List<tblFa_CatPuesto> lstPuesto, int idUsuario)
        {
            return this.FacultamientoDAO.saveFacultamiento(obj, lstAut, lstMonto, lstAuth, lstPuesto, idUsuario);
        }
        public void deleteNoCompleto(string cc)
        {
            this.FacultamientoDAO.deleteNoCompleto(cc);
        }
        public tblFa_CatAuth setAutorizacion(int id, int idUsuario)
        {
            return this.FacultamientoDAO.setAutorizacion(id, idUsuario);
        }
        public tblFa_CatAuth setRechazo(int id, string comentario)
        {
            return FacultamientoDAO.setRechazo(id, comentario);
        }
        public bool remMonto(int id)
        {
            return this.FacultamientoDAO.remMonto(id);
        }
        public bool remAuto(int id)
        {
            return this.FacultamientoDAO.remAuto(id);
        }
        public bool sendCorreo(int iduserRecibe, int iduserEnvia, List<Byte[]> pdf, string cc, int vobo)
        {
            return this.FacultamientoDAO.sendCorreo(iduserRecibe, iduserEnvia, pdf, cc, vobo);
        }
        public List<tblFa_CatFacultamiento> getCuadro()
        {
            return this.FacultamientoDAO.getCuadro();
        }
        public List<tblFa_CatFacultamiento> getCuadroNoR()
        {
            return this.FacultamientoDAO.getCuadroNoR();
        }
        public List<tblFa_CatFacultamiento> getCCCompleto(string cc)
        {
            return this.FacultamientoDAO.getCCCompleto(cc);
        }
        public tblFa_CatFacultamiento getCuadro(string cc, DateTime fecha)
        {
            return this.FacultamientoDAO.getCuadro(cc, fecha);
        }
        public tblFa_CatFacultamiento getCuadro(int id)
        {
            return this.FacultamientoDAO.getCuadro(id);
        }
        public List<tblFa_CatAutorizacion> getAutorizacion(int id, int renglon)
        {
            return this.FacultamientoDAO.getAutorizacion(id, renglon);
        }
        public List<tblFa_CatMonto> getMonto(int id, string cc)
        {
            return this.FacultamientoDAO.getMonto(id, cc);
        }
        public List<tblFa_CatAuth> getLstAuth(int idFacultamiento)
        {
            return this.FacultamientoDAO.getLstAuth(idFacultamiento);
        }
        public List<tblFa_CatPuesto> GetLstPuesto(int idFacultamiento)
        {
            return this.FacultamientoDAO.GetLstPuesto(idFacultamiento);
        }
        public dynamic getLstAutorizacion(int id)
        {
            return this.FacultamientoDAO.getLstAutorizacion(id);
        }
        public dynamic getLstAutorizacion(int id, string nombre)
        {
            return this.FacultamientoDAO.getLstAutorizacion(id, nombre);
        }
        public bool isUsuarioAutorisable(int id, string nombre)
        {
            return this.FacultamientoDAO.isUsuarioAutorisable(id, nombre);
        }
        public CuadroDTO getCuadroFromAutorizado(int idAutorizado)
        {
            return this.FacultamientoDAO.getCuadroFromAutorizado(idAutorizado);
        }
        public bool getAutorizacion(int id)
        {
            return this.FacultamientoDAO.getAutorizacion(id);
        }
        public string geSTtAuth(int id)
        {
            return FacultamientoDAO.geSTtAuth(id);
        }
        public string getPuestoFromNombreCompleto(string completo)
        {
            return this.FacultamientoDAO.getPuestoFromNombreCompleto(completo);
        }
        public string getNombreCC(string cc)
        {
            return this.FacultamientoDAO.getNombreCC(cc);
        }
        public List<object> getComboCC()
        {
            return this.FacultamientoDAO.getComboCC();
        }
        public dynamic getComboCCEnkontrol()
        {
            return this.FacultamientoDAO.getComboCCEnkontrol();
        }
        public dynamic getEmpleadosSigoplan(string term)
        {
            return this.FacultamientoDAO.getEmpleadosSigoplan(term);
        }
        public dynamic getEmpleadosSigoplanNOAG(string term)
        {
            return this.FacultamientoDAO.getEmpleadosSigoplanNOAG(term);
        }
        public List<string> geDesctPuesto(string term)
        {
            return this.FacultamientoDAO.geDesctPuesto(term);
        }

        public string ObtenerMotivoRechazo(int facultamientoID)
        {
            return this.FacultamientoDAO.ObtenerMotivoRechazo(facultamientoID);
        }
        public tblFa_CatFacultamiento getCuadro(int id, int empresa)
        {
            return this.FacultamientoDAO.getCuadro(id, empresa);
        }
        public List<tblFa_CatMonto> getMonto(int id, string cc, int empresa)        
        {
            return this.FacultamientoDAO.getMonto(id, cc, empresa);
        }
        public List<tblFa_CatPuesto> GetLstPuesto(int idFacultamiento, int empresa)        
        {
            return this.FacultamientoDAO.GetLstPuesto(idFacultamiento, empresa);
        }
        public List<tblFa_CatAutorizacion> getAutorizacion(int id, int renglon, int empresa)
        {
            return this.FacultamientoDAO.getAutorizacion(id, renglon, empresa);
        }
    }
}
