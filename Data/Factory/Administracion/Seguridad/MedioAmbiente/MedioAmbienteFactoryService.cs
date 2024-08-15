using Core.DAO.Administracion.Seguridad.MedioAmbiente;
using Core.Service.Administracion.Seguridad.MedioAmbiente;
using Data.DAO.Administracion.Seguridad.MedioAmbiente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.Seguridad.MedioAmbiente
{
    public class MedioAmbienteFactoryService
    {
        public IMedioAmbienteDAO GetMedioAmbienteService()
        {
            return new MedioAmbienteService(new MedioAmbienteDAO());
        }
    }
}
