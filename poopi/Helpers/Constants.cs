using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace poopi.Helpers
{
    public class ConstantsSec
    {
        public static string ImagePath => ConfigurationManager.AppSettings["ImagePath"];
    }
}