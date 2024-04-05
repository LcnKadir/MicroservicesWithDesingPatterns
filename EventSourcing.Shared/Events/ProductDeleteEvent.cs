using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing.Shared.Events
{
    public class ProductDeleteEvent: IEvent
    {
        public Guid Id { get; set; }
    }
}
