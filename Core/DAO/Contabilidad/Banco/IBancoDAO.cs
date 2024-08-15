using Core.DTO.Enkontrol.Tablas.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Banco
{
    public interface IBancoDAO
    {
        sb_cuentaDTO GetBanco(int cta, int scta, int sscta);
    }
}
