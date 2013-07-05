namespace Caitlyn.Test.Linker
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class LinkerHelperFacts
    {
        [TestClass]
        public class TheGetLinkedProjectsMethod
        {
            [TestMethod]
            public void ThrowsArgumentNullExceptionForNullRootProject()
            {
                Assert.Fail("to implement");
            }

            [TestMethod]
            public void ReturnsEmptyArrayForNullRootProjectParentProjectItem()
            {
                Assert.Fail("to implement");
            }

            [TestMethod]
            public void ReturnsLinkedProjects()
            {
                Assert.Fail("to implement");
            }
        }

        [TestClass]
        public class TheStripAllPossibleProjectTargetsMethod
        {
            [TestMethod]
            public void ThrowsArgumentExceptionForNullName()
            {
                Assert.Fail("to implement");
            }

            [TestMethod]
            public void ThrowsArgumentExceptionForEmptyName()
            {
                Assert.Fail("to implement");
            }

            [TestMethod]
            public void ReturnsNameForNameWithoutProjectTarget()
            {
                string input = "Catel.Core";
                string expectedOutput = "Catel.Core";

                Assert.AreEqual(expectedOutput, LinkerHelper.StripAllPossibleProjectTargets(input));
            }

            [TestMethod]
            public void ReturnsNameForNameWithNET35ProjectTarget()
            {
                string input = "Catel.Core.NET35";
                string expectedOutput = "Catel.Core";

                Assert.AreEqual(expectedOutput, LinkerHelper.StripAllPossibleProjectTargets(input));
            }

            [TestMethod]
            public void ReturnsNameForNameWithWP7ProjectTarget()
            {
                string input = "Catel.Core.WP7";
                string expectedOutput = "Catel.Core";

                Assert.AreEqual(expectedOutput, LinkerHelper.StripAllPossibleProjectTargets(input));
            }
        }
    }
}
