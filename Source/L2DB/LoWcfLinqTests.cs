﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using LinqToDB;

namespace Tests.L2DB
{
	class LoWcfLinqTests : TestsWithChangeTrackingBase
	{
		public override string Name => "LoWcf Linq";

		public override void SetUp()
		{
			LoWcfContext.OpenHost();
		}

		public override void TearDown()
		{
			LoWcfContext.Host.Close();
			LoWcfContext.Host = null;
		}

		public LoWcfLinqTests(bool noTracking) : base(noTracking)
		{
		}

		public override bool GetSingleColumnFast(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			using (var db = new LoWcfContext(NoTracking))
				for (var i = 0; i < repeatCount; i++)
					db.Narrows.Where(t => t.ID == 1).Select(t => t.ID).AsEnumerable().First();

			watch.Stop();

			return true;
		}

		public override async Task<bool> GetSingleColumnFastAsync(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			using (var db = new LoWcfContext(NoTracking))
				for (var i = 0; i < repeatCount; i++)
					await db.Narrows.Where(t => t.ID == 1).Select(t => t.ID).FirstAsync();

			watch.Stop();

			return true;
		}

		public override bool GetSingleColumnSlow(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new LoWcfContext(NoTracking))
					db.Narrows.Where(t => t.ID == 1).Select(t => t.ID).AsEnumerable().First();

			watch.Stop();

			return true;
		}

		public override async Task<bool> GetSingleColumnSlowAsync(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new LoWcfContext(NoTracking))
					await db.Narrows.Where(t => t.ID == 1).Select(t => t.ID).FirstAsync();

			watch.Stop();

			return true;
		}

		public override bool GetSingleColumnParam(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			using (var db = new LoWcfContext(NoTracking))
				for (var i = 0; i < repeatCount; i++)
				{
					var id = 1;
					var p  = 2;
					db.Narrows.Where(t => t.ID == id && t.Field1 == p).Select(t => t.ID).AsEnumerable().First();
				}

			watch.Stop();

			return true;
		}

		public override async Task<bool> GetSingleColumnParamAsync(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			using (var db = new LoWcfContext(NoTracking))
				for (var i = 0; i < repeatCount; i++)
				{
					var id = 1;
					var p  = 2;
					await db.Narrows.Where(t => t.ID == id && t.Field1 == p).Select(t => t.ID).FirstAsync();
				}

			watch.Stop();

			return true;
		}

		public override bool GetNarrowList(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new LoWcfContext(NoTracking))
					foreach (var item in db.NarrowLongs.Take(takeCount)) {}

			watch.Stop();

			return true;
		}

		public override async Task<bool> GetNarrowListAsync(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new LoWcfContext(NoTracking))
					await db.NarrowLongs.Take(takeCount).ForEachAsync(item => {});

			watch.Stop();

			return true;
		}

		public override bool GetWideList(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new LoWcfContext(NoTracking))
					foreach (var item in db.WideLongs.Take(takeCount)) {}

			watch.Stop();

			return true;
		}

		public override async Task<bool> GetWideListAsync(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new LoWcfContext(NoTracking))
					await db.WideLongs.Take(takeCount).ForEachAsync(item => {});

			watch.Stop();

			return true;
		}

		public override bool SimpleLinqQuery(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new LoWcfContext(NoTracking))
				{
					var q =
					(
						from n1 in db.Narrows
						where n1.ID < 100
						select n1.ID
					)
					.Take(takeCount);

					foreach (var item in q) {}
				}

			watch.Stop();

			return true;
		}

		public override bool ComplicatedLinqFast(Stopwatch watch, int repeatCount, int takeCount)
		{
			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new LoWcfContext(NoTracking))
				{
					var q =
					(
						from n1 in db.Narrows
						join n2 in db.Narrows on new { n1.ID, n1.Field1 } equals new { n2.ID, n2.Field1 }
						where n1.ID < 100 && n2.Field1 <= 50
						group n1 by n1.ID into gr
						select new
						{
							gr.Key,
							Count = gr.Count()
						}
					)
					.OrderBy(n1 => n1.Key)
					.Skip(1)
					.Take(takeCount);

					foreach (var item in q) {}
				}

			watch.Stop();

			return true;
		}

		public override bool ComplicatedLinqSlow(Stopwatch watch, int repeatCount, int takeCount, int nRows)
		{
			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new LoWcfContext(NoTracking))
				{
					var q =
					(
						from n in db.NarrowLongs
						join w in db.WideLongs on n.Field1 equals w.Field1
						where
							n.ID >= 0 && n.ID <= nRows &&
							!new[] { 0, 20, 50, 187635 }.Contains(w.Field1)
						select new
						{
							n.ID,
							w.Field1
						}
					)
					.Union
					(
						from n in db.NarrowLongs
						join w in db.WideLongs on n.Field1 equals w.Field1
						where
							n.ID >= 0 && n.ID <= nRows &&
							!new[] { 0, 240, 500, 18635 }.Contains(w.Field1)
						select new
						{
							n.ID,
							w.Field1
						}
					)
					.OrderByDescending(n1 => n1.Field1)
					.Skip(1000)
					.Take(takeCount);

					foreach (var item in q) {}
				}

			watch.Stop();

			return true;
		}
	}
}
