using Core.DAO.Maquinaria.Catalogos;
using Core.Service.Maquinaria.Catalogos;
using Data.DAO.Maquinaria.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Maquinaria.Catalogos
{
    public class MaquinaFactoryServices 
    {
        public IMaquinaDAO getMaquinaServices()
        {
            return new MaquinaServices(new MaquinaDAO());
        }
    }
}
