namespace Restaurant.Core
{
	public class Article
	{
		readonly string _name;
		readonly double _price;
		readonly int _timeToBulid;

		public string Name { get { return _name; } }
		public int TimeToBuild { get { return _timeToBulid; } }
		public double Price { get { return _price; } }

		public Article(string name, double price, int timeToBulid)
		{
			_name = name;
			_price = price;
			_timeToBulid = timeToBulid;
		}
	}
}
