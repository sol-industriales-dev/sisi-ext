using Core.DAO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Catalogos
{
    public class MarcaEquipoServices : IMarcaEquipoDAO
    {
                #region Atributos
        private IMarcaEquipoDAO m_maquinariaEquipoDAO;
        #endregion
        #region Propiedades
        public IMarcaEquipoDAO MaquinariaEquipoDAO
        {
            get { return m_maquinariaEquipoDAO; }
            set { m_maquinariaEquipoDAO = value; }
        }
        #endregion
        #region Constructores
        public MarcaEquipoServices(IMarcaEquipoDAO marcaEquipoDAO)
        {
            this.MaquinariaEquipoDAO = marcaEquipoDAO;
        }
        #endregion
         
        public List<MarcaDTO> FillGridMarcaEquipo(tblM_CatMarcaEquipo obj)
        {
            return MaquinariaEquipoDAO.FillGridMarcaEquipo(obj);
        }
        public List<tblM_CatGrupoMaquinaria> FillCboGrupoMaquinaria(bool estatus)
        {
            return MaquinariaEquipoDAO.FillCboGrupoMaquinaria(estatus);
        }

        public tblM_CatGrupoMaquinaria getEntidadGrupo(int idGrupo)
        {
            return MaquinariaEquipoDAO.getEntidadGrupo(idGrupo);
        }

        public void Guardar(tblM_CatMarcaEquipo obj,tblM_CatGrupoMaquinaria grupo)
        {
            MaquinariaEquipoDAO.Guardar(obj, grupo);
        
        }
        public List<tblM_CatMarcaEquipo> GetLstMarcaActivas()
        {
            return MaquinariaEquipoDAO.GetLstMarcaActivas();
        }

        public List<tblM_CatGrupoMaquinaria> GetGruposByMarca(int marcaID)
        {
            return MaquinariaEquipoDAO.GetGruposByMarca(marcaID);
        }

    }
}
