﻿using System.Collections.Generic;
using JulMar.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using JulMar.Core.Collections;

namespace JulMar.Wpf.Helpers.UnitTests.Core
{
    /// <summary>
    ///This is a test class for CollectionObserverTest and is intended
    ///to contain all CollectionObserverTest Unit Tests
    ///</summary>
    [TestClass]
    public class CollectionExtensionTests
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void SortAscendingTest()
        {
            var names = new List<string> {"Zero", "Alice", "Carl", "Mark", "Jim", "Sally", "Adam", "Tom", "Val"};
            string[] sortedNames = names.ToArray();
            Array.Sort(sortedNames);

            names.BubbleSort();
            CollectionAssert.AreEqual(sortedNames, names);
        }

        [TestMethod]
        public void SortDescendingTest()
        {
            var names = new List<string> { "Zero", "Alice", "Carl", "Mark", "Jim", "Sally", "Adam", "Tom", "Val" };
            string[] sortedNames = names.ToArray();
            Array.Sort(sortedNames);
            Array.Reverse(sortedNames);

            names.BubbleSort(true);
            CollectionAssert.AreEqual(sortedNames, names);
        }
    }
}
