using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IocSampleContainer.Tests
{
    [TestClass]
    public class ContainerTests
    {
        [TestMethod]
        public void TransientTest()
        {
            IContainer container = new Container();

            container.Register<Foo>(RegistrationKind.Transient);

            var foo1 = container.Resolve<Foo>();
            var foo2 = container.Resolve<Foo>();

            Assert.AreNotEqual(foo1.Id, foo2.Id);
        }

        [TestMethod]
        public void SingletonTest()
        {
            IContainer container = new Container();

            container.Register<Foo>(RegistrationKind.Singleton);

            var foo1 = container.Resolve<Foo>();
            var foo2 = container.Resolve<Foo>();

            Assert.AreEqual(foo1.Id, foo2.Id);
        }

        [TestMethod]
        public void TransientInterfaceTest()
        {
            IContainer container = new Container();

            container.Register<IFoo, Foo>(RegistrationKind.Transient);

            var foo1 = container.Resolve<IFoo>();
            var foo2 = container.Resolve<IFoo>();

            Assert.AreNotEqual(foo1.Id, foo2.Id);
        }

        [TestMethod]
        public void SingletonInterfaceTest()
        {
            IContainer container = new Container();

            container.Register<IFoo, Foo>(RegistrationKind.Singleton);

            var foo1 = container.Resolve<IFoo>();
            var foo2 = container.Resolve<IFoo>();

            Assert.AreEqual(foo1.Id, foo2.Id);
        }

        [TestMethod]
        public void SameScopeInterfaceTest()
        {
            IContainer container = new Container();

            container.Register<IFoo, Foo>(RegistrationKind.Scope);

            var scope = container.StartNewScope();
            var foo1 = container.Resolve<IFoo>(scope);
            var foo2 = container.Resolve<IFoo>(scope);
            container.FinishScope(scope);

            Assert.AreEqual(foo1.Id, foo2.Id);
        }

        [TestMethod]
        public void DifferentScopeInterfaceTest()
        {
            IContainer container = new Container();

            container.Register<IFoo, Foo>(RegistrationKind.Scope);

            var scope1 = container.StartNewScope();
            var foo1 = container.Resolve<IFoo>(scope1);
            container.FinishScope(scope1);
            var scope2 = container.StartNewScope();
            var foo2 = container.Resolve<IFoo>(scope2);
            container.FinishScope(scope2);

            Assert.AreNotEqual(foo1.Id, foo2.Id);
        }
    }

    public interface IFoo
    {
        Guid Id { get; set; }
    }

    public class Foo : IFoo
    {
        public Foo()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}