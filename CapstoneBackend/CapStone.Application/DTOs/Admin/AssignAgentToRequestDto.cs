using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapStone.Application.DTOs.Admin
{
    public class AssignAgentToRequestDto
    {
        public Guid RequestId { get; set; }
        public Guid AgentId { get; set; }
    }
}
