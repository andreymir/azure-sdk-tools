﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Commands.WAPackIaaS.FunctionalTest
{
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Commands.Utilities.Properties;

    [TestClass]
    public class StopWAPackVMTests : CmdletTestVirtualMachineStatusBase
    {
        [TestMethod]
        [TestCategory("WAPackIaaS")]
        public void ShouldStopExistingVm()
        {
            var vm = VirtualMachine;
            var startedVm = this.SetVirtualMachineState(vm, "Start");
            Assert.AreEqual("Running", startedVm.Properties["StatusString"].Value);

            var stoppedVm = this.SetVirtualMachineState(startedVm, "Stop");
            Assert.AreEqual("Stopped", stoppedVm.Properties["StatusString"].Value);
        }

        [TestMethod]
        [TestCategory("WAPackIaaS")]
        public void ShouldStopExistingVmThatIsAlreadyStopped()
        {
            var vm = VirtualMachine;
            var stoppedVm = this.SetVirtualMachineState(vm, "Stop");
            Assert.AreEqual("Stopped", stoppedVm.Properties["StatusString"].Value);

            stoppedVm = this.SetVirtualMachineState(stoppedVm, "Stop");
            Assert.AreEqual("Stopped", stoppedVm.Properties["StatusString"].Value);
        }

        [TestMethod]
        [TestCategory("WAPackIaaS")]
        public void ShouldShutdownExistingVm()
        {
            var vm = VirtualMachine;
            var startedVm = SetVirtualMachineState(vm, "Start");
            Assert.AreEqual("Running", startedVm.Properties["StatusString"].Value);

            // we need to sleep for a while after starting the vm to ensure we have...
            // access to the service on the OS that allows us to shut down the vm
            Thread.Sleep(50000);
            PowerShell.Commands.Clear();
            PowerShell.AddCommand("Stop-WAPackVM").AddParameter("Shutdown").AddParameter("VM", startedVm);
            var shutdownVm = PowerShell.InvokeAndAssertForNoErrors();
            Assert.AreEqual(1, shutdownVm.Count);
            Assert.AreEqual("Stopped", shutdownVm[0].Properties["StatusString"].Value);
        }

        [TestMethod]
        [TestCategory("WAPackIaaS")]
        [TestCategory("Negative")]
        public void StopVmThatNoLongerExistsFails()
        {
            var vm = VirtualMachine;
            var vmId = vm.Properties["ID"].Value;
            PowerShell.Commands.Clear();
            this.SetVirtualMachineState(vm, "Stop");
            PowerShell.Commands.Clear();
            PowerShell.AddCommand("Remove-WAPackVM")
                .AddParameter("VM", VirtualMachine)
                .AddParameter("Force");
            PowerShell.InvokeAndAssertForNoErrors();

            PowerShell.Commands.Clear();
            PowerShell.AddCommand("Stop-WAPackVM").AddParameter("VM", vm);
            PowerShell.InvokeAndAssertForErrors(string.Format(Resources.OperationFailedErrorMessage, Resources.Stop, vmId));
        }

        [TestMethod]
        [TestCategory("WAPackIaaS")]
        public void ShouldBeAbleToGetAndStopAVmUsingPipeline()
        {
            var vm = VirtualMachine;
            var startedVm = this.SetVirtualMachineState(vm, "Start");
            Assert.AreEqual("Running", startedVm.Properties["StatusString"].Value);

            PowerShell.Commands.Clear();
            PowerShell.AddCommand("Get-WAPackVM")
                .AddParameter("Id", startedVm.Properties["Id"].Value)
                .AddCommand("Stop-WAPackVM");

            var stoppedVm = PowerShell.InvokeAndAssertForNoErrors();
            Assert.AreEqual("Stopped", stoppedVm[0].Properties["StatusString"].Value);
        }

        [TestMethod]
        [TestCategory("WAPackIaaS")]
        [TestCategory("Negative")]
        public void ShutdownVmThatNoLongerExistsFails()
        {
            var vm = VirtualMachine;
            var vmId = vm.Properties["ID"].Value;
            PowerShell.Commands.Clear();
            PowerShell.AddCommand("Remove-WAPackVM").AddParameter("VM", VirtualMachine).AddParameter("Force");
            PowerShell.InvokeAndAssertForNoErrors();

            PowerShell.Commands.Clear();
            PowerShell.AddCommand("Stop-WAPackVM").AddParameter("VM", vm).AddParameter("Shutdown");
            PowerShell.InvokeAndAssertForErrors(string.Format(Resources.OperationFailedErrorMessage, Resources.Shutdown, vmId));
        }
    }
}
