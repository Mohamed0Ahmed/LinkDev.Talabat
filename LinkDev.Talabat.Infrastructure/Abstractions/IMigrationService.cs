﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Abstractions
{
    public interface IMigrationService
    {
        Task UpdateDatabaseAsync();
    }
}
