﻿using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class OrderRequestCompletedEventCunsomer : IOrderRequestCompletedEvent
    {
        public int OrderId { get; set; }
    }
}
