using System;
using NUnit.Framework;

namespace CashMushroom.Application
{
    public class Feature
    {
        protected readonly Guid _1 = Guid.NewGuid();
        protected readonly Guid _2 = Guid.NewGuid();
        protected const String _whiskey = "Jack Daniel's";
        protected const String _candies = "Skittles";
        protected const Decimal _2k = 2000;
        protected const Decimal _500 = 500;
        protected const String _bob = "Bob";
        protected const String _sam = "Sam";

        [SetUp]
        public void SetUp()
        {
            App = new CashMushroom();
        }

        protected CashMushroom App;
    }
}