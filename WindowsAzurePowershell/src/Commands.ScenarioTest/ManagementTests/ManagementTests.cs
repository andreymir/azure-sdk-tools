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

namespace Microsoft.WindowsAzure.Commands.ScenarioTest.ManagementTests
{
    using Common;
    using VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ManagementTests : WindowsAzurePowerShellTest
    {
        public ManagementTests()
            : base("Management\\ManagementTests.ps1")
        {

        }

        #region Remove-AzureSubscription Scenario Tests

        [TestMethod]
        [TestCategory(Category.All)]
        [TestCategory(Category.Management)]
        public void TestRemoveAzureSubscriptionWithDefaultSubscription()
        {
            RunPowerShellTest("Test-RemoveAzureSubscriptionWithDefaultSubscription");
        }

        [TestMethod]
        [TestCategory(Category.All)]
        [TestCategory(Category.Management)]
        public void TestRemoveAzureSubscriptionWithNonExistingSubscription()
        {
            RunPowerShellTest("Test-RemoveAzureSubscriptionWithNonExistingSubscription");
        }

        [TestMethod]
        [TestCategory(Category.All)]
        [TestCategory(Category.Management)]
        public void TestRemoveAzureSubscriptionWithEmptySubscription()
        {
            RunPowerShellTest("Test-RemoveAzureSubscriptionWithEmptySubscription");
        }

        [TestMethod]
        [TestCategory(Category.All)]
        [TestCategory(Category.Management)]
        public void TestRemoveAzureSubscriptionWithWhatIf()
        {
            RunPowerShellTest("Test-RemoveAzureSubscriptionWithWhatIf");
        }

        #endregion
    }
}
