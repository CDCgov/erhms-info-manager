﻿using System;
using System.Diagnostics;

namespace ERHMS.Desktop.Events
{
    public class ProcessStartedEventArgs : EventArgs
    {
        public Process Process { get; }

        public ProcessStartedEventArgs(Process process)
        {
            Process = process;
        }
    }
}