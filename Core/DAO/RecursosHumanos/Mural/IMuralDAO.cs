using Core.DTO;
using Core.Entity.Administrativo.RecursosHumanos.Mural;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.RecursosHumanos.Mural
{
    public interface IMuralDAO
    {
        Respuesta EliminarSeccion(int idSeccion);
        Respuesta GuardarSeccion(tblRH_Mural_Seccion seccion);
        Respuesta EliminarPostIt(int idPostIt);
        Respuesta SavePostIt(tblRH_Mural_PostIt postIt);
        Respuesta CrearMural(tblRH_Mural mural);
        Respuesta GetMural(int idMural);
        Respuesta SaveMural(tblRH_Mural mural, List<tblRH_Mural_PostIt> postIt);
        Dictionary<string, object> CboxMural();

        void setMural(int id,string datos,string icono);
        tblMural_Workspace getMural(int id);

        List<tblMural_Workspace> getMuralList(bool propio);
        void createNewMural(string nombre, string desc);
        void renameMural(int id,string nombre);
        void duplicateNewMural(int id,string nombre, string desc);
        void deleteMural(int id);
        void setUsuarioMural(int idUsuario,int idMural,int tipo);
        void updateUsuarioMural(int id, int tipo);
        void deleteUsuarioMural(int id);
        List<tblMural_Workspace_Members> getUserMuralList(int id);
        int getTipoPermiso(int muralID);
    }
}
