﻿using System;

namespace OperationalService.Operations
{
    public interface IRegularOperationsFactory
    {
        IRegularOperation Create(string name, Action action);
        IRegularOperation Create(string name, Func<long, long> enumeration, Func<long> defaultTimetampFunc);
    }
}