using Core.DTO.Contabilidad.SistemaContable;
using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.SistemaContable
{
    public interface IMesProcDAO
    {
        List<MesProcResumenDTO> getProcesosValidos(SistemasEnkontrolEnum sistema);
        List<MesProcResumenDTO> getProcesosAbiertos(SistemasEnkontrolEnum sistema);
        List<MesProcResumenDTO> getProcesosAbiertosPruebas(SistemasEnkontrolEnum sistema = SistemasEnkontrolEnum.General);
    }
}
