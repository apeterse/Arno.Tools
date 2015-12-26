namespace Arno.Tools.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    using Arno.Tools.Data;

    using NUnit.Framework;

    [TestFixture]
    public class BulkinserterTest
    {
        private static IEnumerable<DataObj> DataSource
        {
            get
            {
                return new List<DataObj>
                       {
                           new DataObj { Name = "User1", Age = 16 },
                           new DataObj { Name = "User2", Age = 26 },
                           new DataObj { Name = "User3", Age = 36 },
                           new DataObj { Name = "User4", Age = 46 }
                       };
            }
        }

        [Test]
        public void TestBulkinsertWithSqlServer()
        {
            SqlConnection connection = new SqlConnection(@"Server=Jupiter\SqlExpress;Database=Issue_Manager;Trusted_Connection=True;");

            SqlBulkCopy bulkCopy = new SqlBulkCopy(connection);
            connection.Open();
            bulkCopy.DestinationTableName = "TestTable";
            bulkCopy.WriteToServer(new GenericDataReader<DataObj>(DataSource));
            connection.Close();
        }
    }

    public class DataObj
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }

    [TestFixture]
    public class EnumerableDataReaderFixture
    {

        private static IEnumerable<DataObj> DataSource
        {
            get
            {
                return new List<DataObj>
                       {
                           new DataObj { Name = "User1", Age = 16 },
                           new DataObj { Name = "User2", Age = 26 },
                           new DataObj { Name = "User3", Age = 36 },
                           new DataObj { Name = "User4", Age = 46 }
                       };
            }
        }

        [Test]
        public void TestIEnumerableCtor()
        {
            var r = new GenericDataReader<DataObj>(DataSource);
            while (r.Read())
            {
                var values = new object[2];
                int count = r.GetValues(values);
                Assert.AreEqual(2, count);

                values = new object[1];
                count = r.GetValues(values);
                Assert.AreEqual(1, count);

                values = new object[3];
                count = r.GetValues(values);
                Assert.AreEqual(2, count);

                Assert.IsInstanceOf(typeof(string), r.GetValue(0));
                Assert.IsInstanceOf(typeof(int), r.GetValue(1));

                Console.WriteLine("Name: {0}, Age: {1}", r.GetValue(0), r.GetValue(1));
            }
        }
    }
}