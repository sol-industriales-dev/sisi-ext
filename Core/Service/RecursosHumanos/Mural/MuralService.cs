using Core.DAO.RecursosHumanos.Mural;
using Core.DTO;
using Core.Entity.Administrativo.RecursosHumanos.Mural;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.RecursosHumanos.Mural
{
    public class MuralService : IMuralDAO
    {
        private IMuralDAO _mural { get; set; }

        private IMuralDAO Mural
        {
            get { return _mural; }
            set { _mural = value; }
        }

        public MuralService(IMuralDAO mural)
        {
            this.Mural = mural;
        }

        public Respuesta EliminarSeccion(int idSeccion)
        {
            return _mural.EliminarSeccion(idSeccion);
        }

        public Respuesta GuardarSeccion(tblRH_Mural_Seccion seccion)
        {
            return _mural.GuardarSeccion(seccion);
        }

        public Respuesta EliminarPostIt(int idPostIt)
        {
            return _mural.EliminarPostIt(idPostIt);
        }

        public Respuesta SavePostIt(tblRH_Mural_PostIt postIt)
        {
            return _mural.SavePostIt(postIt);
        }

        public Respuesta CrearMural(tblRH_Mural mural)
        {
            return _mural.CrearMural(mural);
        }

        public Respuesta GetMural(int idMural)
        {
            return _mural.GetMural(idMural);
        }

        public Respuesta SaveMural(tblRH_Mural mural, List<tblRH_Mural_PostIt> postIt)
        {
            return _mural.SaveMural(mural, postIt);
        }

        public Dictionary<string, object> CboxMural()
        {
            return Mural.CboxMural();
        }

        public void setMural(int id,string datos,string icono)
        {
            _mural.setMural(id,datos,icono);
        }

        public tblMural_Workspace getMural(int id)
        {
            return _mural.getMural(id);
        }

        public List<tblMural_Workspace> getMuralList(bool propio)
        {
            return _mural.getMuralList(propio);
        }
        public void createNewMural(string nombre, string desc)
        {
            _mural.createNewMural(nombre, desc);
        }
        public void renameMural(int id, string nombre) {
            _mural.renameMural(id, nombre);
        }
        public void duplicateNewMural(int id, string nombre, string desc)
        {
            _mural.duplicateNewMural(id, nombre, desc);
        }
        public void deleteMural(int id)
        {
            _mural.deleteMural(id);
        }
        public void setUsuarioMural(int idUsuario, int idMural, int tipo)
        {
            _mural.setUsuarioMural(idUsuario, idMural, tipo);
        }
        public void updateUsuarioMural(int id, int tipo)
        {
            _mural.updateUsuarioMural(id, tipo);
        }
        public void deleteUsuarioMural(int id)
        {
            _mural.deleteUsuarioMural(id);
        }
        public List<tblMural_Workspace_Members> getUserMuralList(int id)
        {
            return _mural.getUserMuralList(id);
        }
        public int getTipoPermiso(int muralID)
        {
            return _mural.getTipoPermiso(muralID);
        }
    }
}
