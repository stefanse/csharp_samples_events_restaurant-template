using System.Collections.Generic;

namespace Restaurant.Core
{
    public class Guest
    {
        public Guest(string name)
        {
            Name = name;
           List<Article> articles = new List<Article>();
        }

        public string Name { get; }
        public IEnumerable<Article> Articles { get; }

        public void OrderArticle(Article article)
        {
            (Articles as List<Article>)?.Add(article);
        }

        public double CalculateSumOfOrders()
        {
            double sum = 0.0;

            foreach (var article in Articles)
            {
                sum += article.Price;
            }
            return sum;
        }
    }
}