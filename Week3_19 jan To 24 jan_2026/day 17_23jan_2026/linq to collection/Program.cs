namespace linq_to_collection
{
    class Program
    {
        static void Main()
        {
            FilterAndProject();
            TopScores();
            GroupAndAggregate();
        }

        static void FilterAndProject()
        {
            var products = new List<string> { "Pen", "Pencil", "Notebook", "Marker", "Paper" };

            var result = products
                .Where(p => p.StartsWith("P"))
                .Select(p => p.ToUpper());

            Console.WriteLine("Products starting with P:");
            foreach (var item in result)
                Console.WriteLine(item);
        }

        static void TopScores()
        {
            var scores = new List<int> { 78, 92, 85, 92, 67, 88, 95 };

            var top3 = scores
                .OrderByDescending(s => s)
                .Distinct()
                .Take(3);

            Console.WriteLine("\nTop 3 Scores:");
            foreach (var score in top3)
                Console.WriteLine(score);
        }

        static void GroupAndAggregate()
        {
            var sales = new List<Sale>
            {
                new Sale("North", 1200),
                new Sale("South", 800),
                new Sale("North", 500),
                new Sale("East", 900),
                new Sale("South", 700)
            };

            var totals = sales
                .GroupBy(s => s.Region)
                .Select(g => new
                {
                    Region = g.Key,
                    Total = g.Sum(x => x.Amount),
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Total);

            Console.WriteLine("\nSales Summary:");
            foreach (var t in totals)
                Console.WriteLine($"{t.Region} -> Total: {t.Total}, Count: {t.Count}");
        }
    }

    record Sale(string Region, decimal Amount);
}

