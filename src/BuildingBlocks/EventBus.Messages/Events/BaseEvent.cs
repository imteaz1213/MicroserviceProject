using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class BaseEvent
    {

        public BaseEvent()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
        }
        public Guid Id { get; private set; }

        public DateTime CreateDate { get; private set; }
    }
}
