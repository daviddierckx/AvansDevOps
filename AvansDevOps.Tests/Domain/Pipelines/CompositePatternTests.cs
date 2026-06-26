using AvansDevOps.Domain.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Pipelines
{
    [TestClass]
    public class CompositePatternTests
    {
        // TC-35: Groep retourneert true als alle acties slagen
        [TestMethod]
        public void PipelineActionGroup_WhenAllSucceed_ReturnsTrue()
        {
            var group = new PipelineActionGroup("Analyse groep");
            group.Add(new BuildAction(true));
            group.Add(new TestAction(true));
            bool result = group.Execute();
            Assert.IsTrue(result);
        }

        // TC-36: Groep stopt en retourneert false bij eerste falende actie
        [TestMethod]
        public void PipelineActionGroup_WhenChildFails_StopsAndReturnsFalse()
        {
            var group = new PipelineActionGroup("Build groep");
            group.Add(new BuildAction(false));
            group.Add(new TestAction(true));
            bool result = group.Execute();
            Assert.IsFalse(result);
        }

        // TC-37: PipelineActionGroup is uitwisselbaar met enkelvoudige PipelineAction
        [TestMethod]
        public void PipelineActionGroup_IsUsableAsPipelineAction()
        {
            PipelineAction action = new PipelineActionGroup("Groep");
            Assert.IsNotNull(action);
            Assert.IsInstanceOfType(action, typeof(PipelineAction));
        }

        // TC-38: Geneste groepen worden correct uitgevoerd
        [TestMethod]
        public void PipelineActionGroup_WithNestedGroup_ExecutesAll()
        {
            var inner = new PipelineActionGroup("Inner groep");
            inner.Add(new AnalyseAction());
            inner.Add(new UtilityAction());

            var outer = new PipelineActionGroup("Outer groep");
            outer.Add(new SourcesAction());
            outer.Add(inner);

            bool result = outer.Execute();
            Assert.IsTrue(result);
        }
    }
}
