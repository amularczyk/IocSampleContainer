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
    }

    public class Foo
    {
        public Guid Id = Guid.NewGuid();
    }
}
