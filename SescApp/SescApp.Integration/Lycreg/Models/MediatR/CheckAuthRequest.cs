using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SescApp.Integration.Lycreg.Models.MediatR
{
    public class CheckAuthRequest : IRequest<bool>
    {
        public required Authorization Auth { get; init; }
    }
}
