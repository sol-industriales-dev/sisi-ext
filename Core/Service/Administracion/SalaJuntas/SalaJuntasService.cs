using Core.DAO.Administracion.SalaJuntas;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.SalaJuntas;
using Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Administracion.SalaJuntas
{
    public class SalaJuntasService : ISalaJuntasDAO
    {
        #region INIT
        public ISalaJuntasDAO salaJuntasDAO { get; set; }
        public SalaJuntasService(ISalaJuntasDAO salaJuntasDAO)
        {
            this.salaJuntasDAO = salaJuntasDAO;
        }
        #endregion

        #region CALENDARIO

        #endregion
    }
}
