using System.Collections.Generic;
using System.Reflection;
using JetBrains.Application;
using JetBrains.Threading;
using NUnit.Framework;

namespace ReSharper.DictionaryHelper.Tests.Properties
{
    /// <summary>
    /// Test environment. Must be in the global namespace.
    /// </summary>
    [SetUpFixture]
    public class TestEnvironmentAssembly : ReSharperTestEnvironmentAssembly
    {
        /// <summary>
        /// Gets the assemblies to load into test environment.
        /// Should include all assemblies which contain components.
        /// </summary>
        private static IEnumerable<Assembly> GetAssembliesToLoad()
        {
            // Test assembly
            yield return Assembly.GetExecutingAssembly();

#warning you need specify a type from the assembly being tested
//            yield return typeof(ReverseStringAction).Assembly;
        }

        public override void SetUp()
        {
            base.SetUp();
            ReentrancyGuard.Current.Execute(
                "LoadAssemblies",
                () => Shell.Instance.GetComponent<AssemblyManager>().LoadAssemblies(
                    GetType().Name, GetAssembliesToLoad()));
        }

        public override void TearDown()
        {
            ReentrancyGuard.Current.Execute(
                "UnloadAssemblies",
                () => Shell.Instance.GetComponent<AssemblyManager>().UnloadAssemblies(
                    GetType().Name, GetAssembliesToLoad()));
            base.TearDown();
        }
    }
}