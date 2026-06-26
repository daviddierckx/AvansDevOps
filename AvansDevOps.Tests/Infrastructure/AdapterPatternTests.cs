using System.Collections.Generic;
using AvansDevOps.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Infrastructure
{
    [TestClass]
    public class AdapterPatternTests
    {
        // TC-49: GitAdapter implementeert IScmProvider correct
        [TestMethod]
        public void GitAdapter_ImplementsIScmProvider()
        {
            IScmProvider provider = new GitAdapter("https://github.com/test/repo");
            Assert.IsNotNull(provider);
            Assert.IsInstanceOfType(provider, typeof(IScmProvider));
        }

        // TC-50: AzureDevOpsAdapter implementeert IScmProvider correct
        [TestMethod]
        public void AzureDevOpsAdapter_ImplementsIScmProvider()
        {
            IScmProvider provider = new AzureDevOpsAdapter("https://dev.azure.com/test/repo");
            Assert.IsNotNull(provider);
            Assert.IsInstanceOfType(provider, typeof(IScmProvider));
        }

        // TC-51: BacklogService roept GetBranches aan op de provider
        [TestMethod]
        public void BacklogService_GetBranches_CallsScmProvider()
        {
            IScmProvider provider = new GitAdapter("https://github.com/test/repo");
            BacklogService service = new BacklogService(provider);

            List<string> branches = service.GetBranches();

            Assert.IsNotNull(branches);
            Assert.IsTrue(branches.Count > 0);
        }

        // TC-52: BacklogService roept GetCommits aan op de provider
        [TestMethod]
        public void BacklogService_GetCommits_CallsScmProvider()
        {
            IScmProvider provider = new GitAdapter("https://github.com/test/repo");
            BacklogService service = new BacklogService(provider);

            List<string> commits = service.GetCommits("main");

            Assert.IsNotNull(commits);
            Assert.IsTrue(commits.Count > 0);
        }

        [TestMethod]
        public void BacklogService_CanSwitchProvider_ToAzureDevOps()
        {
            IScmProvider provider = new AzureDevOpsAdapter("https://dev.azure.com/test/repo");
            BacklogService service = new BacklogService(provider);

            List<string> branches = service.GetBranches();
            Assert.IsNotNull(branches);
            Assert.IsTrue(branches.Count > 0);
        }
    }
}
