﻿using Core.Entity.StarSoft.OrdenCompra;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.StarSoft.Compras
{
    public class ANEXOMapping : EntityTypeConfiguration<ANEXO>
    {
        public ANEXOMapping()
        {
            HasKey(x => x.ANEX_CODIGO);
            ToTable("MAEPROV");
        }
    }
}