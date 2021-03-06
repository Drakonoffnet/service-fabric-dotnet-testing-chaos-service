﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace ChaosTest.ChaosService
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using ChaosTest.Common;
    using Microsoft.ServiceFabric.Services.Runtime;

    public class Program
    {
        private const string FabricCodePath = @"C:\Program Files\Microsoft Service Fabric\bin\Fabric\Fabric.Code";
        private const string ServiceModelAssemblyName = "System.Fabric.Management.ServiceModel";

        public static void Main(string[] args)
        {
            try
            {
                ServiceRuntime.RegisterServiceAsync(
                    StringResource.ChaosTestServiceType,
                    context =>
                        new ChaosService(context)).GetAwaiter().GetResult();

                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromFabricCodePath);

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(ChaosService).Name);

                // "Restart code package" fault - generated by Chaos engine - uses Ctrl+C internally, 
                // and visual studio debugger breaks on Thread.Sleep if Ctrl+C happens;
                // So, if debugger break on the line below, please just hit "Continue".
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e);
                throw;
            }
        }

        private static Assembly LoadFromFabricCodePath(object sender, ResolveEventArgs args)
        {
            string assemblyName = new AssemblyName(args.Name).Name;

            if (!assemblyName.Equals(ServiceModelAssemblyName))
            {
                return null;
            }

            try
            {
                string assemblyPath = Path.Combine(FabricCodePath, assemblyName + ".dll");
                if (File.Exists(assemblyPath))
                {
                    return Assembly.LoadFrom(assemblyPath);
                }
            }
            catch (Exception)
            {
                // Suppress any Exception so that we can continue to
                // load the assembly through other means
            }

            return null;
        }

    }
}