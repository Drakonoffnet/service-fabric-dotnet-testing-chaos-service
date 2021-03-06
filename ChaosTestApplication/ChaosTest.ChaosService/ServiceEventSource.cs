﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace ChaosTest.ChaosService
{
    using System.Diagnostics.Tracing;
    using ChaosTest.Common;

    [EventSource(Name = "MyCompany-ServiceApplication-ChaosTestAppChaosService")]
    internal sealed class ServiceEventSource : CommonServiceEventSource
    {
        public static ServiceEventSource Current = new ServiceEventSource();
    }
}