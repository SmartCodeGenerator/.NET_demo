﻿using BlackCaviarBank.Domain.Core;
using System.Collections.Generic;

namespace BlackCaviarBank.Services.Interfaces
{
    public interface INotifier
    {
        List<Notification> NotifyAll(Service sender, string message);
        Notification Notify(Service sender, UserProfile receiver, string message);
    }
}