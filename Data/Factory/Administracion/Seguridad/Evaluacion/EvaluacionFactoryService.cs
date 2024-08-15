using Core.DAO.Administracion.Seguridad.Evaluacion;
using Core.Service.Administracion.Seguridad.Evaluacion;
using Data.DAO.Administracion.Seguridad.Evaluacion;

namespace Data.Factory.Administracion.Seguridad.Evaluacion
{
    public class EvaluacionFactoryService
    {
        public IEvaluacionDAO GetEvaluacionService()
        {
            return new EvaluacionService(new EvaluacionDAO());
        }
    }
}
