using Core.DTO.Contabilidad;
using Core.DTO.Contabilidad.Bancos;
using Core.DTO.Contabilidad.Propuesta;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Propuesta
{
    public interface IChequesDAO
    {
        List<ChequesDTO> getLstEdoCta(BusqConcentradoDTO busq); 
        List<ComboDTO> cboCuentaBanco();
        List<sbCuentaDTO> getConsultaConcentrado(BusqConsultaConcentradoDTO busq);
        List<sbCuentaDTO> getLstCuenta(List<string> lstCta);
        List<RptBanMovDTO> getLstBancosMov(BusqBancoMov busq);
    }
}
