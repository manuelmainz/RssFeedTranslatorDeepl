﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssFeedTranslator.Utils
{
    public interface ITranslator
    {
        Task<string> Translate(string summary);
    }
}
