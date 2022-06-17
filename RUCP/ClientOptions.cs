﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RUCP
{
    public class ClientOptions
    {
        /// <summary>
        /// Waiting time before writing data to the socket for maximum filling of the MTU. There are 10,000 ticks in a millisecond.
        /// </summary>
        public int SendTimeout { get; set; } = 200;
        /// <summary>
        /// Time to wait for a response from the remote host before disconnecting it
        /// </summary>
        public int DisconnectTimeout { get; set; } = 6_000;
    }
}
