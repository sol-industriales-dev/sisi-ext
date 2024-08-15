using Core.DTO.Maquinaria.Catalogos;
using Core.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using Core.Service.Maquinaria.Catalogos;
using Data.DAO.Maquinaria.Catalogos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Service.Maquinaria;

namespace Data.Factory.Maquinaria.Catalogos
{
    public class MarcaEquipoFactoryServices
    {
        public IMarcaEquipoDAO getMarcaEquipoService()
        {
            return new MarcaEquipoServices(new MarcaEquipoDAO());
        }
    }
}
