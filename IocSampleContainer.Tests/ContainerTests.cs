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

            container.Register<Foo>(false);

            var foo1 = container.Resolve<Foo>();
            var foo2 = container.Resolve<Foo>();

            Assert.AreNotEqual(foo1.Id, foo2.Id);
        }

        [TestMethod]
        public void SingletonTest()
        {
            IContainer container = new Container();

            container.Register<Foo>(true);

            var foo1 = container.Resolve<Foo>();
            var foo2 = container.Resolve<Foo>();

            Assert.AreEqual(foo1.Id, foo2.Id);
        }

        [TestMethod]
        public void TransientInterfaceTest()
        {
            IContainer container = new Container();

            container.Register<IFoo, Foo>(false);

            var foo1 = container.Resolve<IFoo>();
            var foo2 = container.Resolve<IFoo>();

            Assert.AreNotEqual(foo1.Id, foo2.Id);
        }

        [TestMethod]
        public void SingletonInterfaceTest()
        {
            IContainer container = new Container();

            container.Register<IFoo, Foo>(true);

            var foo1 = container.Resolve<IFoo>();
            var foo2 = container.Resolve<IFoo>();

            Assert.AreEqual(foo1.Id, foo2.Id);
        }
    }

    public interface IFoo
    {
        Guid Id { get; set; }
    }

    public class Foo : IFoo
    {
        public Guid Id { get; set; }

        public Foo()
        {
                Id = Guid.NewGuid();
        }
    }
}
