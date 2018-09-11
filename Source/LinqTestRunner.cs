﻿using System;
using System.IO;
using System.Linq;

using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;

using TestRunner;
using TestRunner.DataModel;

namespace Tests
{
	using DataModel;
	using Tests;

	public class LinqTestRunner : TestRunner.TestRunner
	{
		const string DatabaseVersion = "1";

		public static void Run(string platform, bool asyncProcessing)
		{
			Console.WriteLine($"Testing {platform}...");

			var serverName = ".";

			DataConnection.AddConfiguration(
				"Test",
				$"Server={serverName};Database=PerformanceTest;Trusted_Connection=True;Application Name=LinqToDB Test;"
					+ (asyncProcessing ? "Asynchronous Processing=True;" : ""),
				SqlServerTools.GetDataProvider(SqlServerVersion.v2012));

			DataConnection.DefaultConfiguration = "Test";

			var basePath = Path.GetDirectoryName(typeof(LinqTestRunner).Assembly.Location);

			while (!Directory.Exists(Path.Combine(basePath, "Result")))
				basePath = Path.GetDirectoryName(basePath);

			var resultPath = Path.Combine(basePath, "Result");

			CreateResultDatabase(false, resultPath);
			CreateTestDatabase  (false, serverName);
			RunTests(platform);
		}

		static void RunTests(string platform)
		{
//			new L2DB.L2DBLinqTests().GetNarrowList(new Stopwatch(), 10000, 1);
//			return;

			var testProviders = new ITests[]
			{
				new AdoNet  .AdoNetTests    (),
				new Dapper  .DapperTests    (),
				new PetaPoco.PetaPocoTests  (),
				new L2DB    .L2DBSqlTests   (),
				new L2DB    .L2DBLinqTests  (),
				new L2DB    .L2DBCompTests  (),
				new EFCore  .EFCoreSqlTests (),
				new EFCore  .EFCoreLinqTests(),
				new EFCore  .EFCoreCompTests(),
				new BLT     .BLTSqlTests    (),
				new BLT     .BLTLinqTests   (),
				new BLT     .BLTCompTests   (),
				new EF6     .EF6SqlTests    (),
				new EF6     .EF6LinqTests   (),
				new L2S     .L2SSqlTests    (),
				new L2S     .L2SLinqTests   (),
				new L2S     .L2SCompTests   (),
			};

			var testProvidersWithChangeTracking = new ITests[]
			{
				new AdoNet.AdoNetTests     (),
				new L2DB.L2DBLinqTests     { TrackChanges = true },
				new L2DB.L2DBCompTests     { TrackChanges = true },
				new EFCore.EFCoreSqlTests  { TrackChanges = true },
				new EFCore.EFCoreLinqTests { TrackChanges = true },
				new EFCore.EFCoreCompTests { TrackChanges = true },
				new EF6.EF6SqlTests        { TrackChanges = true },
				new EF6.EF6LinqTests       { TrackChanges = true },
				new L2S.L2SSqlTests        { TrackChanges = true },
				new L2S.L2SLinqTests       { TrackChanges = true },
				new L2S.L2SCompTests       { TrackChanges = true },
			};

			RunTests(platform, "Single Column", testProviders.OfType<ISingleColumnTests>(), new[]
			{
				CreateTest<ISingleColumnTests>(t => t.GetSingleColumnFast,  10000),
				CreateTest<ISingleColumnTests>(t => t.GetSingleColumnSlow,  10000),
				CreateTest<ISingleColumnTests>(t => t.GetSingleColumnParam, 10000),
			});

			RunTests(platform, "Single Column with Change Tracking", testProvidersWithChangeTracking.OfType<ISingleColumnTests>(), new[]
			{
				CreateTest<ISingleColumnTests>(t => t.GetSingleColumnFast,  10000),
				CreateTest<ISingleColumnTests>(t => t.GetSingleColumnSlow,  10000),
				CreateTest<ISingleColumnTests>(t => t.GetSingleColumnParam, 10000),
			});

			RunTests(platform, "Single Column Async", testProviders.OfType<ISingleColumnAsyncTests>(), new[]
			{
				CreateTest<ISingleColumnAsyncTests>(t => t.GetSingleColumnFastAsync,  10000),
				CreateTest<ISingleColumnAsyncTests>(t => t.GetSingleColumnSlowAsync,  10000),
				CreateTest<ISingleColumnAsyncTests>(t => t.GetSingleColumnParamAsync, 10000),
			});

			RunTests(platform, "Single Column Async with Change Tracking", testProvidersWithChangeTracking.OfType<ISingleColumnAsyncTests>(), new[]
			{
				CreateTest<ISingleColumnAsyncTests>(t => t.GetSingleColumnFastAsync,  10000),
				CreateTest<ISingleColumnAsyncTests>(t => t.GetSingleColumnSlowAsync,  10000),
				CreateTest<ISingleColumnAsyncTests>(t => t.GetSingleColumnParamAsync, 10000),
			});

			RunTests(platform, "Narrow List", testProviders.OfType<IGetListTests>(), new[]
			{
				CreateTest<IGetListTests>(t => t.GetNarrowList,           10000,   1),
				CreateTest<IGetListTests>(t => t.GetNarrowList,           10000,  10),
				CreateTest<IGetListTests>(t => t.GetNarrowList,           10000, 100),
				CreateTest<IGetListTests>(t => t.GetNarrowList,           1000, 1000),
				CreateTest<IGetListTests>(t => t.GetNarrowList,           100, 10000),
				CreateTest<IGetListTests>(t => t.GetNarrowList,           10, 100000),
				CreateTest<IGetListTests>(t => t.GetNarrowList,           1, 1000000),
			});

			RunTests(platform, "Narrow List with Change Tracking", testProvidersWithChangeTracking.OfType<IGetListTests>(), new[]
			{
				CreateTest<IGetListTests>(t => t.GetNarrowList,           10000,  1),
				CreateTest<IGetListTests>(t => t.GetNarrowList,           10000, 10),
				CreateTest<IGetListTests>(t => t.GetNarrowList,           1000, 100),
				CreateTest<IGetListTests>(t => t.GetNarrowList,           1000, 1000),
				CreateTest<IGetListTests>(t => t.GetNarrowList,           100, 1000),
				CreateTest<IGetListTests>(t => t.GetNarrowList,           10, 10000),
				CreateTest<IGetListTests>(t => t.GetNarrowList,           1, 100000),
//				CreateTest<IGetListTests>(t => t.GetNarrowList,           1, 1000000),
			});

			RunTests(platform, "Narrow List Async", testProviders.OfType<IGetListAsyncTests>(), new[]
			{
				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 10000,   1),
				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 10000,  10),
				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 10000, 100),
				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 1000, 1000),
				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 100, 10000),
				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 10, 100000),
				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 1, 1000000),
			});

			RunTests(platform, "Narrow List Async with Change Tracking", testProvidersWithChangeTracking.OfType<IGetListAsyncTests>(), new[]
			{
				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 10000,  1),
				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 10000, 10),
				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 1000, 100),
				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 1000, 1000),
				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 100, 1000),
				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 10, 10000),
				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 1, 100000),
//				CreateTest<IGetListAsyncTests>(t => t.GetNarrowListAsync, 1, 1000000),
			});

			RunTests(platform, "Wide List", testProviders.OfType<IGetListTests>(), new[]
			{
				CreateTest<IGetListTests>(t => t.GetWideList,             10000,   1),
				CreateTest<IGetListTests>(t => t.GetWideList,             10000,  10),
				CreateTest<IGetListTests>(t => t.GetWideList,             10000, 100),
				CreateTest<IGetListTests>(t => t.GetWideList,             1000, 1000),
				CreateTest<IGetListTests>(t => t.GetWideList,             100, 10000),
				CreateTest<IGetListTests>(t => t.GetWideList,             10, 100000),
				CreateTest<IGetListTests>(t => t.GetWideList,             1, 1000000),
			});

			RunTests(platform, "Wide List with Change Tracking", testProvidersWithChangeTracking.OfType<IGetListTests>(), new[]
			{
				CreateTest<IGetListTests>(t => t.GetWideList,             1000,   1),
				CreateTest<IGetListTests>(t => t.GetWideList,             1000,  10),
				CreateTest<IGetListTests>(t => t.GetWideList,             1000, 100),
				CreateTest<IGetListTests>(t => t.GetWideList,             1000, 1000),
				CreateTest<IGetListTests>(t => t.GetWideList,             100, 1000),
				CreateTest<IGetListTests>(t => t.GetWideList,             10, 10000),
				CreateTest<IGetListTests>(t => t.GetWideList,             1, 100000),
//				CreateTest<IGetListTests>(t => t.GetWideList,             1, 1000000),
			});

			RunTests(platform, "Wide List Async", testProviders.OfType<IGetListAsyncTests>(), new[]
			{
				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   10000,   1),
				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   10000,  10),
				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   10000, 100),
				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   1000, 1000),
				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   100, 10000),
				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   10, 100000),
				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   1, 1000000),
			});

			RunTests(platform, "Wide List Async with Change Tracking", testProvidersWithChangeTracking.OfType<IGetListAsyncTests>(), new[]
			{
				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   1000,   1),
				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   1000,  10),
				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   1000, 100),
				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   1000, 1000),
				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   100, 1000),
				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   10, 10000),
				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   1, 100000),
//				CreateTest<IGetListAsyncTests>(t => t.GetWideListAsync,   1, 1000000),
			});

			RunTests(platform, "Linq Query", testProviders.OfType<ILinqQueryTests>(), new[]
			{
				CreateTest<ILinqQueryTests>(t => t.SimpleLinqQuery,     1000,  1),
				CreateTest<ILinqQueryTests>(t => t.ComplicatedLinqFast, 1000,  1),
				CreateTest<ILinqQueryTests>(t => t.ComplicatedLinqSlow,  100, 10, 100),
				CreateTest<ILinqQueryTests>(t => t.ComplicatedLinqSlow,  100, 10, 1000),
				CreateTest<ILinqQueryTests>(t => t.ComplicatedLinqSlow,   20, 10, 250000),
				CreateTest<ILinqQueryTests>(t => t.ComplicatedLinqSlow,   10, 10, 500000),
			});

#if !NETCOREAPP2_0
			var wcfTestProviders = new ITests[]
			{
				new AdoNet.AdoNetTests (),
				new L2DB.L2DBLinqTests (),
				new EF6.EF6LinqTests   { TrackChanges = true },
				new L2DB.LoWcfLinqTests(),
			};

			RunTests(platform, "Linq over WCF Single Column", wcfTestProviders.OfType<ISingleColumnTests>(), new[]
			{
				CreateTest<ISingleColumnTests>(t => t.GetSingleColumnFast,  1000),
				CreateTest<ISingleColumnTests>(t => t.GetSingleColumnSlow,  1000),
				CreateTest<ISingleColumnTests>(t => t.GetSingleColumnParam, 1000),
			});

			RunTests(platform, "Linq over WCF Narrow List", wcfTestProviders.OfType<IGetListTests>(), new[]
			{
				CreateTest<IGetListTests>(t => t.GetNarrowList,        1000,   1),
				CreateTest<IGetListTests>(t => t.GetNarrowList,        1000,  10),
				CreateTest<IGetListTests>(t => t.GetNarrowList,        1000, 100),
				CreateTest<IGetListTests>(t => t.GetNarrowList,        100, 1000),
				CreateTest<IGetListTests>(t => t.GetNarrowList,        10, 10000),
				CreateTest<IGetListTests>(t => t.GetNarrowList,        1, 100000),
			});

			RunTests(platform, "Linq over WCF Wide List", wcfTestProviders.OfType<IGetListTests>(), new[]
			{
				CreateTest<IGetListTests>(t => t.GetWideList,          1000,   1),
				CreateTest<IGetListTests>(t => t.GetWideList,          1000,  10),
				CreateTest<IGetListTests>(t => t.GetWideList,          1000, 100),
				CreateTest<IGetListTests>(t => t.GetWideList,          100, 1000),
				CreateTest<IGetListTests>(t => t.GetWideList,          10, 10000),
			});

			RunTests(platform, "Linq over WCF Linq Query", wcfTestProviders.OfType<ILinqQueryTests>(), new[]
			{
				CreateTest<ILinqQueryTests>(t => t.SimpleLinqQuery,      1000,  1),
				CreateTest<ILinqQueryTests>(t => t.ComplicatedLinqFast,  1000,  1),
				CreateTest<ILinqQueryTests>(t => t.ComplicatedLinqSlow,    20, 10, 250000),
				CreateTest<ILinqQueryTests>(t => t.ComplicatedLinqSlow,    10, 10, 500000),
			});
#endif

			RunTests("All", "LinqToDB Compare",
			new[]
			{
				new L2DB.L2DBAllTests(platform, L2DB.TestType.AdoNet),
				new L2DB.L2DBAllTests(platform, L2DB.TestType.Linq),
				new L2DB.L2DBAllTests(platform, L2DB.TestType.Async),
				new L2DB.L2DBAllTests(platform, L2DB.TestType.ChangeTracking),
				new L2DB.L2DBAllTests(platform, L2DB.TestType.ChangeTrackingAsync),
			},
			new[]
			{
				CreateTest<L2DB.L2DBAllTests>(t => t.GetList, 1000, 1000),
			});

			RunTests("All", "EF Core Compare",
			new[]
			{
				new EFCore.EFCoreAllTests(platform, EFCore.TestType.AdoNet),
				new EFCore.EFCoreAllTests(platform, EFCore.TestType.Linq),
				new EFCore.EFCoreAllTests(platform, EFCore.TestType.Async),
				new EFCore.EFCoreAllTests(platform, EFCore.TestType.ChangeTracking),
				new EFCore.EFCoreAllTests(platform, EFCore.TestType.ChangeTrackingAsync),
			},
			new[]
			{
				CreateTest<EFCore.EFCoreAllTests>(t => t.GetList, 1000, 1000),
			});
		}

		static void CreateTestDatabase(bool enforceCreate, string serverName)
		{
			Console.WriteLine("Creating database...");

			using (var db = SqlServerTools.CreateDataConnection($"Server={serverName};Database=master;Trusted_Connection=True"))
			{
				if (!enforceCreate)
					if (db.Execute<object>("SELECT db_id('PerformanceTest')") != null)
						if (db.Execute<object>("SELECT OBJECT_ID('PerformanceTest.dbo.Setting', N'U')") != null)
							if (db.GetTable<Setting>()
								.DatabaseName("PerformanceTest")
								.Any(s => s.Name == "DB Version" && s.Value == DatabaseVersion))
								return;

				db.Execute("DROP DATABASE IF EXISTS PerformanceTest");
				db.Execute("CREATE DATABASE PerformanceTest");
			}

			using (var db = new L2DB.L2DBContext())
			{
				CreateTable(db, new[] { new Setting { Name = "DB Version", Value =  DatabaseVersion } });
				CreateTable(db, new[] { new Narrow { ID = 1, Field1 = 2 } });
				CreateTable(db, Enumerable.Range(1, 1000000).Select(i => new NarrowLong { ID = i, Field1 = -i }));
				CreateTable(db, Enumerable.Range(1, 1000000).Select(i => new WideLong
				{
					ID            = i,
					Field1        = -i,
					ByteValue     = i % 2 == 0 ? null : (byte?)    (i % Byte.MaxValue  / 2),
					ShortValue    = i % 2 == 0 ? null : (short?)   (i % Int16.MaxValue / 2),
					IntValue      = i % 2 == 1 ? null : (int?)     (i % Int32.MaxValue - 1),
					LongValue     = i % 2 == 0 ? null : (long?)    (i * 2),
					StringValue   = i % 2 == 0 ? null : new string(Enumerable.Range(0, 95).Select(n => (char)(n % 30 + (int)' ')).ToArray()),
					DateTimeValue = i % 2 == 1 ? null : (DateTime?)new DateTime(i),
					TimeValue     = i % 2 == 1 ? null : (TimeSpan?)new TimeSpan(i),
					DecimalValue  = i % 2 == 0 ? null : (decimal?)i,
					DoubleValue   = i % 2 == 1 ? null : (double?)i,
					FloatValue    = i % 2 == 0 ? null : (float?)i,
				}));
			}

			Console.WriteLine("Database created.");
		}
	}
}
