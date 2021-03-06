﻿using System;

namespace cleanLayer.Library.Scripts
{
    public class TerminateException : Exception
    {
        public TerminateException()
            : base()
        { }

        public TerminateException(string reason)
            : base()
        { }

        public string Reason
        {
            get;
            private set;
        }
    }
}
