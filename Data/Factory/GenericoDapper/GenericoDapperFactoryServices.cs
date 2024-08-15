using Core.DAO.GenericoDapper;
using Core.Service.GenericoDapper;
using Data.DAO.GenericoDapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.GenericoDapper
{
    public class GenericoDapperFactoryServices
    {
        public IGenericoDapper GenericoDapperDAO()
        {
            return new GenericoDapperService(new GenericoDapperDAO());
        }
    }
}
