﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakeMe.Core.Entities;

namespace TakeMe.Core.Interfaces
{
    public interface IRegisterDaily:IGenericRepositrie<RegisterDaily>
    {
        Task<bool> IsAreadyExist(string UserId);
    }
}
