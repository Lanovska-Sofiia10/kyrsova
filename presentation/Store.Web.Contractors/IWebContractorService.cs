﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Web.Contractors
{
    public interface IWebContractorService
    {
        string UniqueCode {  get; }

        string GetUri { get; }


    }
}