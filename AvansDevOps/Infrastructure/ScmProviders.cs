using System;
using System.Collections.Generic;

namespace AvansDevOps.Infrastructure
{
    // ============================================================
    // ADAPTER PATTERN
    // Doel: Definieer een gemeenschappelijke interface IScmProvider
    // in de Application Core. Concrete adapters in de Infrastructure-
    // laag vertalen aanroepen naar de specifieke externe API.
    // OO-principe: Dependency Inversion — BacklogService hangt af van
    // de abstractie IScmProvider, niet van GitAdapter of AzureDevOpsAdapter.
    // ============================================================

    public interface IScmProvider
    {
        string GetRepository();
        List<string> GetBranches();
        List<string> GetCommits(string branch);
    }

    // Adapter voor Git — volledig geïmplementeerd
    public class GitAdapter : IScmProvider
    {
        private string _repoUrl;

        public GitAdapter(string repoUrl) { _repoUrl = repoUrl; }

        public string GetRepository()
        {
            Console.WriteLine("[GIT] Repository ophalen: " + _repoUrl);
            return _repoUrl;
        }

        public List<string> GetBranches()
        {
            Console.WriteLine("[GIT] Branches ophalen voor: " + _repoUrl);
            return new List<string> { "main", "develop", "feature/login" };
        }

        public List<string> GetCommits(string branch)
        {
            Console.WriteLine("[GIT] Commits ophalen voor branch: " + branch);
            return new List<string> { "abc123 - Initial commit", "def456 - Add login feature" };
        }
    }

    // Adapter voor Azure DevOps — stub (provider is vervangbaar via interface)
    public class AzureDevOpsAdapter : IScmProvider
    {
        private string _projectUrl;

        public AzureDevOpsAdapter(string projectUrl) { _projectUrl = projectUrl; }

        public string GetRepository()
        {
            Console.WriteLine("[AZURE] Repository ophalen: " + _projectUrl);
            return _projectUrl;
        }

        public List<string> GetBranches()
        {
            Console.WriteLine("[AZURE] Branches ophalen voor: " + _projectUrl);
            return new List<string> { "main", "release/1.0" };
        }

        public List<string> GetCommits(string branch)
        {
            Console.WriteLine("[AZURE] Commits ophalen voor branch: " + branch);
            return new List<string> { "commit-001 - Setup project", "commit-002 - Add pipeline" };
        }
    }

    // Service in Application Core — gebruikt uitsluitend IScmProvider
    public class BacklogService
    {
        private IScmProvider _scmProvider;

        public BacklogService(IScmProvider scmProvider)
        {
            _scmProvider = scmProvider;
        }

        public List<string> GetBranches()
        {
            return _scmProvider.GetBranches();
        }

        public List<string> GetCommits(string branch)
        {
            return _scmProvider.GetCommits(branch);
        }

        public string GetRepository()
        {
            return _scmProvider.GetRepository();
        }
    }
}
