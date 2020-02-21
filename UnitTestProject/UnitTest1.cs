using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Helpers;

namespace UnitTestProject
{
    /// <summary>
    /// test classes for Posts
    /// </summary>
    [TestClass]
    public class UnitTestPosts
    {
        /// <summary>
        /// Not Exists id
        /// </summary>
        [TestMethod]
        public void TestMethod_NotExistUser()
        {
            clsDataBaseMethods objDataBase = new clsDataBaseMethods();

            Assert.IsFalse(objDataBase.ExistsId("-1"));
        }

        /// <summary>
        /// Exists post id
        /// </summary>
        [TestMethod]
        public void TestMethod_ExistUser()
        {
            clsDataBaseMethods objDataBase = new clsDataBaseMethods();

            Assert.IsTrue(objDataBase.ExistsId("1"));
        }

    }
}
