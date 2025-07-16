using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JourneyMate.Application.DTOs
{
    public class QueryPramDto
    {
        public string? SearchPhrase { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

    }
}
