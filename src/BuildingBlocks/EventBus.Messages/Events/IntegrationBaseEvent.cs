﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public IntegrationBaseEvent()
        {
                Id = Guid.NewGuid();
                CreationDate = DateTime.Now;
        }
        public IntegrationBaseEvent(Guid Id,DateTime createDate)
        {
            this.Id = Id;
            this.CreationDate = createDate;
        }
    }
}
