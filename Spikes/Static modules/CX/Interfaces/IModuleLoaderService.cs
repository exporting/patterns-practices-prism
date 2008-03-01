﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CX.Interfaces
{
    public interface IModuleLoaderService
    {
        ModuleMetadata[] LookForModules();
        void InitializeModules();
    }
}