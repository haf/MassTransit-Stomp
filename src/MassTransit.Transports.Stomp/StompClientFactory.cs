﻿// Copyright 2011 Ernst Naezer, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.

namespace MassTransit.Transports.Stomp
{
    using System;
    using Ultralight.Client;

    public class StompClientFactory
    {
        /// <summary>
        /// Builds a stomp client for connecting to the given address
        /// </summary>
        public static Func<string, StompClient> Build =  address =>
                                                             {
                                                                    throw new Exception("Please configure the Stomp Client Transport factory before calling the build function");
                                                             };
    }
}