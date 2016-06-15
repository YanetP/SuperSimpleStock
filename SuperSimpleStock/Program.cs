using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSimpleStock
{
    class Program
    {
        public class Stock
        {
            public string Symbol { get; set; }
            public string Type { get; set; }
            public int LastDividend { get; set; }
            public double FixedDividend { get; set; }
            public int PartValue { get; set; }
        }

        public class Record
        {
            public string symbol { get; set; }
            public DateTime timestamp { get; set; }
            public int shares { get; set; }
            public string buySell { get; set; }
            public double price { get; set; }
        }

        static void Main(string[] args)
        {
            //Specify the ticker price
            double tickerPrice = 9.0;
            //Specify the Stock Symbol that needs to be tested
            string arg = "GIN";
            Dictionary<int, Stock> stockList = new Dictionary<int, Stock>()
        {
            {1, new Stock {Symbol="TEA" , Type="Common", LastDividend = 0, PartValue=100 } },
            {2, new Stock {Symbol="POP", Type="Common", LastDividend =8,PartValue=100 } },
            {3, new Stock {Symbol="ALE", Type="Common", LastDividend=23,  PartValue=60 } },
            {4, new Stock {Symbol="GIN", Type="Preferred", LastDividend=8, FixedDividend=0.02,PartValue=100 } },
            {5, new Stock {Symbol="JOE", Type="Common", LastDividend=13, PartValue=250 } }
        };
            double dividendYield = 0.0;
            double pERatio = 0.0;
            double geometricMean = 0.0;
            double stockPrice = 0.0;
            List<Record> tradeRecord = new List<Record>();
            //For the specified symbol the Dividen Yield, P/E Ratio, Stock Price and the Geometric Mean will be calculated and displayed as well a list of some records of the trade generated randomly.
            //The result will be displayed.
            foreach (KeyValuePair<int, Stock> kvp in stockList)
            {
                if (kvp.Value.Symbol.Equals(arg.ToUpper()))
                {
                    dividendYield = DividendYield(kvp.Value, tickerPrice);
                    pERatio = PERatio(kvp.Value, tickerPrice);
                    tradeRecord = RecordTrade(arg);
                    stockPrice = StokePrice(tradeRecord, arg);
                    geometricMean = GBCE(tradeRecord, arg);
                }
            }

            Console.WriteLine("For the stock: " + args);
            Console.WriteLine("The Dividend Yield is: " + dividendYield);
            Console.WriteLine("The P/E Ratio is : " + pERatio);
            Console.WriteLine("The Stcok Price is: " + stockPrice);
            Console.WriteLine("The records for the trade in the las 20 minutes are: ");
            foreach (Record r in tradeRecord)
            {
                Console.WriteLine(r.symbol + " " + r.timestamp + " " + r.shares + " " + r.buySell + " " + r.price);
            }
            Console.WriteLine("The stock price is: " + stockPrice);
            Console.WriteLine("The GBCE is: " + geometricMean);
            Console.ReadKey();
     }
        //Calculate the Dividend Yield
        public static double DividendYield(Stock stock, double tickerPrice)
        {

            double result = 0;
            try
            {
                if (tickerPrice > 0)
                {
                    if (stock.Type.ToUpper().Equals("PREFERRED"))
                    {
                        result = (stock.FixedDividend * stock.PartValue) / tickerPrice;
                    }
                    else
                    {
                        result = stock.LastDividend / tickerPrice;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }

        //Calculate the P/E Ratio
        public static double PERatio(Stock stock, double tickerPrice)
        {
            double result = 0;
            double divided = 0;
            try
            {
                divided = DividendYield(stock, tickerPrice);
                if (divided > 0)
                {
                    result = tickerPrice / divided;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return result;
        }

        //Calculates the records for a trade
        public static List<Record> RecordTrade(string arg)
        {
            List<Record> record = new List<Record>();
            Random rdm = new Random();
            string[] str = { "buy", "sell" };
            int itt = rdm.Next(str.Count());
            for (int i = 0; i <= 20; i++)
            {
                Record r = new Record();
                r.symbol = arg;
                r.timestamp = DateTime.Now.AddMinutes(i);
                r.shares = rdm.Next(50);
                r.buySell = str[itt];
                r.price = rdm.NextDouble();
                record.Add(r);
            }

            return record;
        }

        //Calculates the stock price
        public static double StokePrice(List<Record> list, string arg)
        {
            DateTime date = DateTime.Now;
            double sumQuan = 0;
            double price = 0.0;
            double result = 0.0;
            foreach (Record r in list)
            {
                if (r.symbol.ToUpper().Equals(arg))
                {
                    double d = (r.timestamp - date).TotalMinutes;
                    if (d <= 15)
                    {
                        price += (r.shares * r.price);
                        sumQuan += r.shares;

                    }
                }
            }
            if (sumQuan > 0)
                result = price / sumQuan;

            return result;
        }

        //Calculates the GBCE
        public static double GBCE(List<Record> record, string arg)
        {
            double gbce = 0.0;
            double prices = 0.0;
            foreach (Record r in record)
            {
                if (r.symbol.ToUpper().Equals(arg.ToUpper()))
                {
                    prices += r.price;
                }
            }

            return gbce = Math.Pow(prices, 1.0 / record.Count);
        }

    }
}
