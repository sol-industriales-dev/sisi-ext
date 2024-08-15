using Core.DAO.Administracion.DocumentosXPagar;
using Core.DTO;
using Core.Enum.Multiempresa;
using Core.Service.Administracion.DocumentosXPagar;
using Data.DAO.Administracion.DocumentosXPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Administracion.DocumentosXPagar
{
    public class DocumentosXPagarFactoryServices
    {
        public IContratosDAO GetDocumentosXPagarServices()
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    return new ContratosService(new DocumentosXPagarPeruDAO());
                    break;
                case EmpresaEnum.Colombia:
                    return new ContratosService(new DocumentosXPagarColombiaDAO());
                    break;
                default:
                    return new ContratosService(new DocumentosXPagarDAO());
                    break;
            }
        }
    }
}