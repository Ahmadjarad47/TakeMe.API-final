﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMe.Core.Entities
{
    public class BaseEntity<T>
    {
        public T Id { get; set; }
    }
}
