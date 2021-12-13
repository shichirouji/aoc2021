static void Main(string[] args)
{
	DayTwelve();
}

static void DayTwelve()
{
	string[] input = System.IO.File.ReadAllLines(@"C:\Test\Twelve.txt");

	List<string> cavePairs = input.ToList();

	List<string> reversedPairs = new List<string>();
	foreach (string cavePair in cavePairs)
	{
		string[] caves = cavePair.Split("-");
		reversedPairs.Add($"{caves.Last()}-{caves.First()}");
	}
	cavePairs.AddRange(reversedPairs);

	int partOnePathCount = 0;
	int partTwoPathCount = 0;
	for (int i = 0; i < cavePairs.Count; ++i)
	{
		List<string> paths = new List<string>();

		if (cavePairs[i].StartsWith("start"))
		{
			paths.AddRange(GetPaths(cavePairs[i], cavePairs));

			foreach (string path in paths) 
			{
				string[] caves = path.Split("-");

				Dictionary<string, int> caveVisitCount = new Dictionary<string, int>();
				foreach (string cave in caves)
				{
					if (!caveVisitCount.ContainsKey(cave)) caveVisitCount.Add(cave, 0);
					caveVisitCount[cave]++;
				}

				bool isCorrectPathPartOne = true;
				bool isCorrectPathPartTwo = true;

				int visitTwiceCount = caveVisitCount.Keys.Where(cave => cave.Any(char.IsLower) && caveVisitCount[cave] > 1).Count();
				if (visitTwiceCount > 0) isCorrectPathPartOne = false;
				if (visitTwiceCount > 1) isCorrectPathPartTwo = false;

				if (isCorrectPathPartOne) partOnePathCount++;
				if (isCorrectPathPartTwo) partTwoPathCount++;
			}
		}
	}

	Console.WriteLine($"Part I: {partOnePathCount}");
	Console.WriteLine($"Part II: {partTwoPathCount}");
}

static List<string> GetPaths(string leftPart, List<string> cavePairs)
{
	List<string> paths = new List<string>();
	foreach (string cavePair in cavePairs)
	{
		string[] caves = cavePair.Split("-");

		string path = string.Empty;
		if (leftPart.Split("-").Last() == caves.First())
		{
			path = $"{leftPart}-{caves.Last()}";
			paths.Add(path);
		}

		if (path.Length > 0)
		{
			string[] pathCaves = path.Split("-");

			Dictionary<string, int> caveVisitCount = new Dictionary<string, int>();
			foreach (string cave in pathCaves)
			{
				if (!caveVisitCount.ContainsKey(cave)) caveVisitCount.Add(cave, 0);
				caveVisitCount[cave]++;
			}

			bool continuePathGeneration = true;
			foreach (string cave in caveVisitCount.Keys)
			{
				if ((cave.Contains("start") || cave.Contains("end")) && caveVisitCount[cave] > 1) continuePathGeneration = false;
				else if (cave.Any(char.IsLower) && caveVisitCount[cave] > 2) continuePathGeneration = false;
			}
			if (caveVisitCount.Keys.Where(cave => cave.Any(char.IsLower) && caveVisitCount[cave] > 1).Count() > 1) continuePathGeneration = false;

			if (continuePathGeneration && !path.EndsWith("end")) paths.AddRange(GetPaths(path, cavePairs));
		}
	}
	return paths.Where(x => x.StartsWith("start") && x.EndsWith("end")).ToList();
}