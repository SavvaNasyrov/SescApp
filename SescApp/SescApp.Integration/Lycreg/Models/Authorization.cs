using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SescApp.Integration.Lycreg.Models
{
    public record Authorization
    {
        public required string Login { get; init; }

        public required string Token { get; init; }

        public required string ShortRole { get; init; }

        public required IReadOnlyCollection<LycregRole> Roles { get; init; }

        public IReadOnlyDictionary<string, string>? TeachLoad { get; init; }
    }
}
