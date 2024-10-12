﻿using LinkDev.Talabat.Core.Application.Abstraction.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstraction.Services
{
    public interface IServiceManager
    {
        public IProductService ProductService { get;  }
    }
}