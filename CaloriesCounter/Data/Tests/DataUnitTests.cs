using CaloriesCounter.Data.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloriesCounter.Data.Tests
{
    class DataUnitTests
    {
        /// <summary>
        /// Tests the lifecycle of a day
        /// </summary>
        [TestMethod]
        public void FullLifeCycleTest()
        {
            DataSource ds = new DataSource();
            ds.InitDatabase();
            ds.CreateTables();

            Day d = new Day
            {
                Date = DateTime.Today,
                Total = 100,
                Protein = 0
            };

            ds.Days.Create(d).GetAwaiter().GetResult();

            var lookup = ds.Days.Items.Where(x => x.Total == 100)
                .ToListAsync().GetAwaiter().GetResult().First();

            Assert.IsNotNull(lookup);

            lookup.Total = 200;
            ds.Days.Update(lookup).GetAwaiter().GetResult();

            var lookup2 = ds.Days.Items.Where(x => x.Total == 200)
                .ToListAsync().GetAwaiter().GetResult().First();

            Assert.IsNotNull(lookup2);

            ds.Days.Delete(lookup2).GetAwaiter().GetResult();

            var lookup3 = ds.Days.Items.Where(x => x.Total == 200)
                .ToListAsync().GetAwaiter().GetResult();

            Assert.AreEqual(lookup3.Count, 0);

        }
    }
}
