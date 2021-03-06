﻿using System.ComponentModel.Composition;
using JulMar.Core;
using JulMar.Core.Interfaces;
using JulMar.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace JulMar.Wpf.Helpers.UnitTests
{
    /// <summary>
    ///This is a test class for ServiceProviderTest and is intended
    ///to contain all ServiceProviderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceProviderTest
    {
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            const string targetString = "This is a test";

            IServiceLocator sp = ServiceLocator.Instance;

            sp.Add(typeof(string), targetString);

            var result = sp.Resolve<string>();
            Assert.AreEqual(targetString, result);
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveTest()
        {
            const string targetString = "This is a test";

            IServiceLocator sp = ServiceLocator.Instance;

            sp.Add(typeof(string), targetString);

            var result = sp.Resolve<string>();
            Assert.AreEqual(targetString, result);

            sp.Remove(typeof(string));
            result = sp.Resolve<string>();
            Assert.AreEqual(null, result);
        }

        /// <summary>
        ///A test for Add with subtype
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidCastTest()
        {
            object obj = new object();
            IServiceLocator sp = ServiceLocator.Instance;

            sp.Add(typeof(IComparable), obj);

            var result = sp.Resolve<string>();
            Assert.AreEqual(null, result);

            sp.Resolve<IComparable>();
        }

        [Export(typeof(MyServiceClass))]
        class MyServiceClass
        {
        }

        [TestMethod]
        public void TestMyServiceExport()
        {
            IServiceLocator sp = DynamicComposer.Instance.GetExportedValue<IServiceLocator>();
            Assert.IsNotNull(sp);

            var msc = sp.Resolve<MyServiceClass>();
            Assert.IsNotNull(msc);
        }

        interface IMyServiceClass2 {}

        [Export(typeof(IMyServiceClass2))]
        class MyServiceClass2 : IMyServiceClass2
        {
        }

        [TestMethod]
        public void TestMyServiceExportInterface()
        {
            IServiceLocator sp = DynamicComposer.Instance.GetExportedValue<IServiceLocator>();
            Assert.IsNotNull(sp);

            var msc = sp.Resolve<IMyServiceClass2>();
            Assert.IsNotNull(msc);
        }

        [TestMethod]
        public void TestMyServiceExportInterfaceInherited()
        {
            IServiceLocator sp = DynamicComposer.Instance.GetExportedValue<IServiceLocator>();
            Assert.IsNotNull(sp);

            var msc = sp.Resolve<IMyServiceClassBase>();
            Assert.IsNull(msc);
        }

        [InheritedExport]
        interface IMyServiceClassBase { }

        interface IMyServiceClass3 : IMyServiceClassBase { }

        [Export(typeof(IMyServiceClass3))]
        class MyServiceClass3 : IMyServiceClass3
        {
        }

        private class TestImportClass
        {
            [Import]
            internal IMyServiceClass3 TheTestImport = null;
        }

        [TestMethod]
        public void ImportTestClass()
        {
            TestImportClass theClass = new TestImportClass();
            DynamicComposer.Instance.Compose(theClass);
            Assert.IsNotNull(theClass.TheTestImport);
        }
    }
}
