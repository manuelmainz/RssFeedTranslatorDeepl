﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssFeedTranslator.Utils
{
    public interface IPersister
    {
        void Store(object data);

        TValue? Load<TValue>();
    }
}
