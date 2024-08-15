using Core.DAO.Contabilidad.SistemaContable;
using Core.DTO.Contabilidad.SistemaContable;
using Core.DTO.Utils.Data;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Data.EntityFramework.Context;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Contabilidad.SistemaContable
{
    public class MesProcDAO : IMesProcDAO
    {
        public List<MesProcResumenDTO> getProcesosValidos(SistemasEnkontrolEnum sistema = SistemasEnkontrolEnum.General)
        {
            var consulta = string.Format("SELECT * FROM \"DBA\".\"sc_mesproc\" WHERE {0} = 'S'", sistema.GetDescription());
            var proceso = _contextEnkontrol.Select<MesProcDTO>(EnkontrolAmbienteEnum.Prod, consulta);
            var lst = proceso.Select(s => new MesProcResumenDTO(sistema, s)).ToList();
            return lst;
        }
        public List<MesProcResumenDTO> getProcesosAbiertos(SistemasEnkontrolEnum sistema = SistemasEnkontrolEnum.General)
        {
            var consulta = string.Format("SELECT * FROM \"DBA\".\"sc_mesproc\" WHERE {0} = 'N'", sistema.GetDescription());
            var proceso = _contextEnkontrol.Select<MesProcDTO>(EnkontrolAmbienteEnum.Prod, consulta);
            var lst = proceso.Select(s => new MesProcResumenDTO(sistema, s)).ToList();
            return lst;
        }
        public List<MesProcResumenDTO> getProcesosAbiertosPruebas(SistemasEnkontrolEnum sistema = SistemasEnkontrolEnum.General)
        {
            var consulta = string.Format("SELECT * FROM \"DBA\".\"sc_mesproc\" WHERE {0} = 'N'", sistema.GetDescription());
            var proceso = _contextEnkontrol.Select<MesProcDTO>(EnkontrolAmbienteEnum.Prueba, consulta);
            var lst = proceso.Select(s => new MesProcResumenDTO(sistema, s)).ToList();
            return lst;
        }
    }
}
