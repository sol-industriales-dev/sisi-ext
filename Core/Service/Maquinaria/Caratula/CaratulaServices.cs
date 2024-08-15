using Core.DAO.Maquinaria.Caratula;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.DTO.Principal.Generales;
using Core.DTO.Maquinaria.Caratula;


namespace Core.Service.Maquinaria
{
    public class CaratulaServices : ICaratulaDAO
    {

        private ICaratulaDAO m_ICaratulaDAO;       

        private ICaratulaDAO CaratulaDAO
        {
            get { return m_ICaratulaDAO; }
            set { m_ICaratulaDAO = value; }
        }
        public CaratulaServices(ICaratulaDAO CaratulaDAO)
        {
            this.CaratulaDAO = CaratulaDAO;
        }

        public List<ComboDTO> FillAreasCuentas()
        {
            return CaratulaDAO.FillAreasCuentas();
        }

        public List<ComboDTO> FillCboModelo()
        {
            return CaratulaDAO.FillCboModelo();
        }

        public List<ComboDTO> FillCboGrupo()
        {
            return CaratulaDAO.FillCboGrupo();
        }

        public List<CaratulaDTO> GetCaratula()
        {
            return CaratulaDAO.GetCaratula();
        }
    }
}
