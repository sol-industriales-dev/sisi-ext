
using Core.DAO.Maquinaria.Caratulas;
using Core.DTO.Maquinaria.Caratulas;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Caratulas
{
    public class CaratulasService : ICaratulasDAO
    {
         private ICaratulasDAO m_ICaratulaDAO;       

        private ICaratulasDAO CaratulaDAO
        {
            get { return m_ICaratulaDAO; }
            set { m_ICaratulaDAO = value; }
        }
        public CaratulasService(ICaratulasDAO CaratulaDAO)
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

        public List<CaratulasDTO> GetCaratula()
        {
            return CaratulaDAO.GetCaratula();
        }
    }
}
