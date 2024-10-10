using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Application.Abstraction.Interfaces
{
    public interface ILoggedInUserService
    {
        public string? UserId { get;  }
    }
}

