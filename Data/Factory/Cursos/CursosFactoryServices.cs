using Core.DAO.Cursos;
using Core.Service.Cursos;
using Data.DAO.Cursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.Factory.Cursos
{
    public class CursosFactoryServices
    {
        public ICursoDAO getCursosService() 
        {
            return new CursoService(new  CursosDAO());
        }
    }
}
