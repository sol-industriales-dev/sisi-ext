using Core.DTO.Administracion.Facultamiento;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Facultamiento;
using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Administracion.Facultamiento
{
    public interface IFacultamientoDAO

    {

        List<tblFa_CatFacultamiento> getCCCompleto(List<string> cc, DateTime fechaI, DateTime fechaF);
        tblFa_CatAuth saveFacultamiento(tblFa_CatFacultamiento obj, List<tblFa_CatAutorizacion> lstAut, List<tblFa_CatMonto> lstMonto, List<tblFa_CatAuth> lstAuth, List<tblFa_CatPuesto> lstPuesto, int idUsuario);
        void deleteNoCompleto(string cc);
        tblFa_CatAuth setAutorizacion(int id, int idUsuario);
        tblFa_CatAuth setRechazo(int id, string comentario);
        bool remMonto(int id);
        bool remAuto(int id);
        bool sendCorreo(int iduserRecibe, int iduserEnvia, List<Byte[]> pdf, string cc, int vobo);
        List<tblFa_CatFacultamiento> getCuadro();
        List<tblFa_CatFacultamiento> getCuadroNoR();
        List<tblFa_CatFacultamiento> getCCCompleto(string cc);
        tblFa_CatFacultamiento getCuadro(string cc, DateTime fecha);
        tblFa_CatFacultamiento getCuadro(int id);
        List<tblFa_CatAutorizacion> getAutorizacion(int id, int renglon);
        List<tblFa_CatMonto> getMonto(int id, string cc);
        List<tblFa_CatAuth> getLstAuth(int idFacultamiento);
        List<tblFa_CatPuesto> GetLstPuesto(int idFacultamiento);
        CuadroDTO getCuadroFromAutorizado(int idAutorizado);
        bool getAutorizacion(int id);
        string geSTtAuth(int id);
        dynamic getLstAutorizacion(int id);
        dynamic getLstAutorizacion(int id, string nombre);
        bool isUsuarioAutorisable(int id, string nombre);
        string getPuestoFromNombreCompleto(string completo);
        string getNombreCC(string cc);
        List<object> getComboCC();
        dynamic getComboCCEnkontrol();
        dynamic getEmpleadosSigoplan(string term);
        dynamic getEmpleadosSigoplanNOAG(string term);
        List<string> geDesctPuesto(string term);
        string ObtenerMotivoRechazo(int facultamientoID);
        tblFa_CatFacultamiento getCuadro(int id, int empresa);
        List<tblFa_CatMonto> getMonto(int id, string cc, int empresa);
        List<tblFa_CatPuesto> GetLstPuesto(int idFacultamiento, int empresa);
        List<tblFa_CatAutorizacion> getAutorizacion(int id, int renglon, int empresa);
    }
}
