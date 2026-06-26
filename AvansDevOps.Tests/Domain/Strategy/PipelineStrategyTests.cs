using AvansDevOps.Domain.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvansDevOps.Tests.Domain.Strategy
{
    [TestClass]
    public class PipelineStrategyTests
    {
        private Pipeline BuildPipeline(bool firstFails)
        {
            var pipeline = new Pipeline("Test pipeline");
            pipeline.AddPipelineAction(new BuildAction(firstFails ? false : true));
            pipeline.AddPipelineAction(new TestAction(true));
            return pipeline;
        }

        // TC-30: SequentialStrategy gaat door na fout en retourneert false
        [TestMethod]
        public void SequentialStrategy_WhenActionFails_ContinuesAndReturnsFalse()
        {
            var pipeline = BuildPipeline(firstFails: true);
            pipeline.SetStrategy(new SequentialPipelineStrategy());
            bool result = pipeline.ExecuteWithStrategy();
            Assert.IsFalse(result);
        }

        // TC-31: SequentialStrategy retourneert true bij alle successen
        [TestMethod]
        public void SequentialStrategy_WhenAllActionsSucceed_ReturnsTrue()
        {
            var pipeline = BuildPipeline(firstFails: false);
            pipeline.SetStrategy(new SequentialPipelineStrategy());
            bool result = pipeline.ExecuteWithStrategy();
            Assert.IsTrue(result);
        }

        // TC-32: FastFailStrategy stopt direct en retourneert false
        [TestMethod]
        public void FastFailStrategy_WhenActionFails_StopsAndReturnsFalse()
        {
            var pipeline = BuildPipeline(firstFails: true);
            pipeline.SetStrategy(new FastFailPipelineStrategy());
            bool result = pipeline.ExecuteWithStrategy();
            Assert.IsFalse(result);
        }

        // TC-33: FastFailStrategy retourneert true bij alle successen
        [TestMethod]
        public void FastFailStrategy_WhenAllActionsSucceed_ReturnsTrue()
        {
            var pipeline = BuildPipeline(firstFails: false);
            pipeline.SetStrategy(new FastFailPipelineStrategy());
            bool result = pipeline.ExecuteWithStrategy();
            Assert.IsTrue(result);
        }

        // TC-34: Strategie is verwisselbaar zonder Pipeline aan te passen
        [TestMethod]
        public void Pipeline_CanSwitchStrategy_WithoutChangingPipeline()
        {
            var pipeline = BuildPipeline(firstFails: false);

            pipeline.SetStrategy(new FastFailPipelineStrategy());
            bool result1 = pipeline.ExecuteWithStrategy();

            pipeline.SetStrategy(new SequentialPipelineStrategy());
            bool result2 = pipeline.ExecuteWithStrategy();

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
        }
    }
}
