using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcing.Shared.Events
{
    public class ProductNameChangeEvent:IEvent
    {
        public Guid Id { get; set; }
        public string ChangeName { get; set; }
    }
}
