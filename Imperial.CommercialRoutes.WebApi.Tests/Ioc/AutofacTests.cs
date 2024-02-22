using Autofac;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace Imperial.CommercialRoutes.WebApi.Tests.Ioc
{
    /// <summary>
    /// Test class to test proper behaviour of dependency injection with newer Autofac.dll.
    /// </summary>
    public class AutofacTests
    {
        public interface IDependency { }
        public class DependencyA : IDependency { }
        public class DependencyB : IDependency { }
        public class DependencyC : IDependency { }

        [Fact, Description("Exercises a problem in a previous version, to make sure older Autofac.dll is not picked up")]
        public void EnumerablesFromDifferentLifetimeScopesShouldReturnDifferentCollections()
        {
            var rootBuilder = new ContainerBuilder();
            rootBuilder
                .RegisterType<DependencyA>()
                .As<IDependency>();
            var rootContainer = rootBuilder.Build();

            var scopeA = rootContainer.BeginLifetimeScope(
                            scopeBuilder => scopeBuilder
                                                .RegisterType<DependencyB>()
                                                .As<IDependency>());

            var arrayA = scopeA.Resolve<IEnumerable<IDependency>>().ToArray();

            var scopeB = rootContainer.BeginLifetimeScope(
                            scopeBuilder => scopeBuilder
                                                .RegisterType<DependencyC>()
                                                .As<IDependency>());
            var arrayB = scopeB.Resolve<IEnumerable<IDependency>>().ToArray();

            arrayA.Count().Should().Be(2);
            arrayA.Should().Contain(x => x is DependencyA);
            arrayA.Should().Contain(x => x is DependencyB);

            arrayB.Count().Should().Be(2);
            arrayB.Should().Contain(x => x is DependencyA);
            arrayB.Should().Contain(x => x is DependencyC);
        }
    }
}
