﻿using System;

namespace Tests
{
	using Tests;

	namespace BLT
	{
		class BLTCompTests : TestsBase { public override string Name { get; set; } = "BLT Compiled"; }
		class BLTLinqTests : TestsBase { public override string Name { get; set; } = "BLT Linq";     }
		class BLTSqlTests  : TestsBase { public override string Name { get; set; } = "BLT Sql";      }
	}

	namespace EF6
	{
		class EF6LinqTests : TestsBase, IWithChangeTracking { public override string Name { get; set; } = "EF6 Linq"; public bool TrackChanges { get; set; } }
		class EF6SqlTests  : TestsBase, IWithChangeTracking { public override string Name { get; set; } = "EF6 Sql";  public bool TrackChanges { get; set; } }
	}

	namespace L2DB
	{
		class LoWcfLinqTests : TestsBase { public override string Name { get; set; } = "LoWcf Linq"; }
	}

	namespace L2S
	{
		class L2SCompTests : TestsBase, IWithChangeTracking { public override string Name { get; set; } = "L2S Compiled"; public bool TrackChanges { get; set; } }
		class L2SLinqTests : TestsBase, IWithChangeTracking { public override string Name { get; set; } = "L2S Linq";     public bool TrackChanges { get; set; } }
		class L2SSqlTests  : TestsBase, IWithChangeTracking { public override string Name { get; set; } = "L2S Sql";      public bool TrackChanges { get; set; } }
	}
}
