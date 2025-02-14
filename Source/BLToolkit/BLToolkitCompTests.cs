﻿using System;
using System.Diagnostics;
using System.Linq;

using BLToolkit.Data.Linq;

namespace Tests.BLToolkit
{
	using Tests;

	class BLToolkitCompTests : TestsBase, ISingleColumnTests, IGetListTests, ILinqQueryTests
	{
		public override string Name { get; set; } = "BLToolkit Compiled";

		public bool GetSingleColumnFast(Stopwatch watch, int repeatCount, int takeCount)
		{
			var query = CompiledQuery.Compile((BLTContext db) =>
				db.Narrows.Where(t => t.ID == 1).Select(t => t.ID).First());

			watch.Start();

			using (var db = new BLTContext())
				for (var i = 0; i < repeatCount; i++)
					query(db);

			watch.Stop();

			return true;
		}

		public bool GetSingleColumnSlow(Stopwatch watch, int repeatCount, int takeCount)
		{
			var query = CompiledQuery.Compile((BLTContext db) =>
				db.Narrows.Where(t => t.ID == 1).Select(t => t.ID).First());

			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new BLTContext())
					query(db);

			watch.Stop();

			return true;
		}

		public bool GetSingleColumnParam(Stopwatch watch, int repeatCount, int takeCount)
		{
			var query = CompiledQuery.Compile((BLTContext db, int id, int p) =>
				db.Narrows.Where(t => t.ID == id && t.Field1 == p).Select(t => t.ID).First());

			watch.Start();

			using (var db = new BLTContext())
				for (var i = 0; i < repeatCount; i++)
					query(db, 1, 2);

			watch.Stop();

			return true;
		}

		public bool GetNarrowList(Stopwatch watch, int repeatCount, int takeCount)
		{
			var query = CompiledQuery.Compile((BLTContext db, int top) =>
				db.NarrowLongs.Take(top));

			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new BLTContext())
					foreach (var item in query(db, takeCount)) {}

			watch.Stop();

			return true;
		}

		public bool GetWideList(Stopwatch watch, int repeatCount, int takeCount)
		{
			var query = CompiledQuery.Compile((BLTContext db, int top) =>
				db.WideLongs.Take(top));

			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new BLTContext())
					foreach (var item in query(db, takeCount)) {}

			watch.Stop();

			return true;
		}

		public bool SimpleLinqQuery(Stopwatch watch, int repeatCount)
		{
			var query = CompiledQuery.Compile((BLTContext db) =>
				(
					from n1 in db.Narrows
					where n1.ID < 100
					select n1.ID
				));

			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new BLTContext())
					foreach (var item in query(db)) {}

			watch.Stop();

			return true;
		}

		public bool SimpleLinqQueryTop(Stopwatch watch, int repeatCount, int takeCount)
		{
			var query = CompiledQuery.Compile((BLTContext db, int top) =>
				(
					from n1 in db.Narrows
					where n1.ID < 100
					select n1.ID
				)
				.Take(top));

			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new BLTContext())
					foreach (var item in query(db, takeCount)) {}

			watch.Stop();

			return true;
		}

		public bool ComplicatedLinqFast(Stopwatch watch, int repeatCount, int takeCount)
		{
			var query = CompiledQuery.Compile((BLTContext db, int top) =>
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
				.Take(top));

			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new BLTContext())
					foreach (var item in query(db, takeCount)) {}

			watch.Stop();

			return true;
		}

		public bool ComplicatedLinqSlow(Stopwatch watch, int repeatCount, int takeCount, int nRows)
		{
			var query = CompiledQuery.Compile((BLTContext db, int top) =>
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
				.Take(top));

			watch.Start();

			for (var i = 0; i < repeatCount; i++)
				using (var db = new BLTContext())
					foreach (var item in query(db, takeCount)) {}

			watch.Stop();

			return true;
		}
	}
}
