﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EssentialTools.Models;

namespace EssentialTools.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private IDiscountHelper getTestObject()
        {
            return new MinimumDiscountHelper();
        }
        [TestMethod]
        public void Discount_Above_100()
        {
            IDiscountHelper target = getTestObject();
            decimal total = 200;
            var discountedTotal = target.ApplyDiscount(total);
            Assert.AreEqual(total * 0.9M, discountedTotal);
        }
        [TestMethod]
        public void Discount_Between_10_And_100()
        {
            IDiscountHelper target = getTestObject();
            decimal TenDollarDiscount = target.ApplyDiscount(10);
            decimal HundredDollarDiscount = target.ApplyDiscount(100);
            decimal FiftyDollarDiscount = target.ApplyDiscount(50);


            Assert.AreEqual(5, TenDollarDiscount, "$10 discount iswrong");
            Assert.AreEqual(95, HundredDollarDiscount, "$100 discount iswrong");
            Assert.AreEqual(45, FiftyDollarDiscount, "$50 discount iswrong");
        }

        [TestMethod]
        public void Discount_Less_Than_10()
        {
            IDiscountHelper target = getTestObject();
            decimal FiveDollarDiscount = target.ApplyDiscount(5);
            decimal ZoreDollarDiscount = target.ApplyDiscount(0);
            Assert.AreEqual(5, FiveDollarDiscount);
            Assert.AreEqual(0, ZoreDollarDiscount);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Discount_Negative_Total()
        {
            IDiscountHelper target = getTestObject();
            target.ApplyDiscount(-1);
        }

    }
}