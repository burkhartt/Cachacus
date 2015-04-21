using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cachacus.Tests {
    [TestClass]
    public class NoKeyTests {
        [TestMethod]
        public void When_an_object_does_not_have_a_CacheKey_then_no_exception_should_be_thrown() {
            try {
                new CachedCarRepository(new CarRepository());
            } catch (Exception ex) {
                Assert.Fail();
            }
        }
    }
}