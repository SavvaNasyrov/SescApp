using SescApp.Integration.Lycreg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SescApp.Shared.Models
{
    public class UserStorageModel
    {
        public required Authorization? LycregAuth { get; set; }
        public static UserStorageModel Default => new() { LycregAuth = null };
    }
}
