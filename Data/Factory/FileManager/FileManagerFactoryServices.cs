using Core.DAO.FileManager;
using Core.Service.FileManager;
using Data.DAO.FileManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.FileManager
{
    public class FileManagerFactoryServices
    {
        public IFileManagerDAO getFileManagerService()
        {
            return new FileManagerService(new FileManagerDAO());
        }
    }
}
