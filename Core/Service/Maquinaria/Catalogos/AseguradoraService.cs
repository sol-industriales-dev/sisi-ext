using Core.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Catalogos
{
    public class AseguradoraService : IAseguradoraDAO
    {
        #region Atributos
        private IAseguradoraDAO m_aseguradoraDAO;
        #endregion

        #region Propiedades
        public IAseguradoraDAO AseguradoraDAO
        {
            get { return m_aseguradoraDAO; }
            set { m_aseguradoraDAO = value; }
        }
        #endregion

        #region Constructores
        public AseguradoraService(IAseguradoraDAO aseguradoraDAO)
        {
            this.AseguradoraDAO = aseguradoraDAO;
        }
        #endregion
        public void Guardar(tblM_CatAseguradora obj)
        {
            AseguradoraDAO.Guardar(obj);
        }
        public List<tblM_CatAseguradora> FillGridAseguradora(tblM_CatAseguradora obj)
        {
            return AseguradoraDAO.FillGridAseguradora(obj);
        }

    }
}
