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
    public class TipoMaquinariaService : ITipoMaquinaDAO
    {
        #region Atributos
            private ITipoMaquinaDAO m_tipoMaquinaDAO;
            #endregion
        #region Propiedades
        public ITipoMaquinaDAO TipoMaquinaDAO
        {
            get { return m_tipoMaquinaDAO; }
            set { m_tipoMaquinaDAO = value; }
        }
        #endregion
        #region Constructores
        public TipoMaquinariaService(ITipoMaquinaDAO tipoMaquinaDAO)
        {
            this.TipoMaquinaDAO = tipoMaquinaDAO;
        }
        #endregion
        public List<tblM_CatTipoMaquinaria> FillGridTipoMaquinaria(tblM_CatTipoMaquinaria tipoMaquinaria)
        {
            return TipoMaquinaDAO.FillGridTipoMaquinaria(tipoMaquinaria);
        }

        public void Guardar(tblM_CatTipoMaquinaria obj)
        {
            TipoMaquinaDAO.Guardar(obj);
        }
    }
}
