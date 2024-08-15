using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO.Contabilidad;
using System.Web.Mvc;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.Contabilidad;
using Core.DTO.Contabilidad;

namespace Core.Service.Contabilidad
{
    public class ConciliacionCCService : IConciliacionCCDAO
    {
          public IConciliacionCCDAO conciliacionCCDAO { get; set; }

          public ConciliacionCCService(IConciliacionCCDAO conciliacionCCDAO)
        {
            this.conciliacionCCDAO = conciliacionCCDAO;
        }

          public Dictionary<string, object> FillCCPrincipal()
          {              
              return conciliacionCCDAO.FillCCPrincipal();
          }
          public Dictionary<string, object> FillCCSecundario()
          {
              return conciliacionCCDAO.FillCCSecundario();
          }
          public Dictionary<string, object> GetBuscarConciliacionCC(List<string>palEmpresaCC)
          {
              return conciliacionCCDAO.GetBuscarConciliacionCC(palEmpresaCC);
          }
          public Dictionary<string, object> EliminarConciliacionCC(int id)
          {
              return conciliacionCCDAO.EliminarConciliacionCC(id);
          }
          public Dictionary<string, object> GuardarEditarConciliacionCC(tblC_Cta_RelCC data)
          {
              return conciliacionCCDAO.GuardarEditarConciliacionCC(data);
          }
         
    }
}
