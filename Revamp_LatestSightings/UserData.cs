using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revamp_LatestSightings
{
    public class UserData
    {
        public Boolean sessionIsValid { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
    }
}