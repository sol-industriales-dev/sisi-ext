using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad
{
    public interface IConciliacionCCDAO
    {
        Dictionary<string, object> FillCCPrincipal();
        Dictionary<string, object> FillCCSecundario();
        Dictionary<string, object> GetBuscarConciliacionCC(List<string> palEmpresaCC);
        Dictionary<string, object> GuardarEditarConciliacionCC(tblC_Cta_RelCC data);
        Dictionary<string, object> EliminarConciliacionCC(int id);



    }
}
