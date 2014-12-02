using System.Collections.Generic;
using System.Reflection;
using JetBrains.Application;
using JetBrains.TestFramework;
#if RESHARPER9
using JetBrains.TestFramework.Application.Zones;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.TestFramework;
using JetBrains.ReSharper.Resources.Shell;
#endif
using JetBrains.Threading;
using NUnit.Framework;
using ReSharper.DictionaryHelper;

/// <summary>
/// Test environment. Must be in the global namespace.
/// </summary>
// ReSharper disable once CheckNamespace
#if !RESHARPER9
public class TestEnvironmentAssembly : ReSharperTestEnvironmentAssembly
#else
public class TestsZone : ITestsZone
{
}
[SetUpFixture]
public class TestEnvironmentAssembly : TestEnvironmentAssembly<TestsZone>
#endif
{
    /// <summary>
    /// Gets the assemblies to load into test environment.
    /// Should include all assemblies which contain components.
    /// </summary>
    private static IEnumerable<Assembly> GetAssembliesToLoad()
    {
        // Test assembly
        yield return Assembly.GetExecutingAssembly();
        yield return typeof (DictionaryContainsKeyFix).Assembly;
    }

    public override void SetUp()
    {
        base.SetUp();
        ReentrancyGuard.Current.Execute(
            "LoadAssemblies",
            () => Shell.Instance.GetComponent<AssemblyManager>().LoadAssemblies(
                GetType().Name,
                GetAssembliesToLoad()));
    }

    public override void TearDown()
    {
        ReentrancyGuard.Current.Execute(
            "UnloadAssemblies",
            () => Shell.Instance.GetComponent<AssemblyManager>().UnloadAssemblies(
                GetType().Name,
                GetAssembliesToLoad()));
        base.TearDown();
    }
}
