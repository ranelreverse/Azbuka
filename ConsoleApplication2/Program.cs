using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static Dictionary<long, long> _cache = new Dictionary<long, long>();

        static void Main(string[] args)
        {
            int port = 8080;
            
            if (args.Count() != 0)  {
                port = int.Parse(args[0]);
            }

            
            var uri = string.Format("http://127.0.0.1:{0}/nearestPrime/", port);

            var listener = new HttpListener();
            listener.Prefixes.Add(uri);
            listener.Start();
            Console.WriteLine("Listening...");

            while (true)
            {
                try
                {
                    var context = listener.GetContext();
                    var request = context.Request;
                    var queryString = request.QueryString;
                    var response = context.Response;

                    var s = new Stopwatch();
                    s.Start();
                    var number = queryString[0];
                    var responseString = GetPrimeNumber(number).ToString();
                    s.Stop();

                    var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                    response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
                catch
                {
                    Console.WriteLine("Error: Call example - http://127.0.0.1:[port]/nearestPrime?number=[int or long]");  
                }

            }

        }

        private static long GetPrimeNumber(string p)
        {
            var x = long.Parse(p);

            if (_cache.ContainsKey(x))
                return _cache[x];

            var ns = GetNextNumber(x);
            foreach (long n in ns)
            {
                if (TestFerma(n))
                {
                    _cache[x] = n;
                    return n;
                }
            }

            throw new Exception("A prime number not found");
        }

        private static IEnumerable GetNextNumber(long n)
        {
            while (true)
                yield return ++n;
        }

        static bool TestFerma(long x)
        {
            if (x == 2)
                return true;

            for (var i = 0; i < 100; i++)
            {
                long a = (new Random().Next() % (x - 2)) + 2;

                if (Gcd(a, x) != 1)
                    return false;

                if (Pows(a, x - 1, x) != 1)
                    return false;
            }
            return true;
        }

        static long Gcd(long a, long b)
        {
            if (b == 0)
                return a;

            return Gcd(b, a % b);
        }

        static long Mul(long a, long b, long m)
        {
            if (b == 1)
                return a;

            if (b % 2 == 0)
            {
                long t = Mul(a, b / 2, m);
                return (2 * t) % m;
            }

            return (Mul(a, b - 1, m) + a) % m;
        }

        static long Pows(long a, long b, long m)
        {
            if (b == 0)
                return 1;

            if (b % 2 == 0)
            {
                long t = Pows(a, b / 2, m);
                return Mul(t, t, m) % m;
            }

            return (Mul(Pows(a, b - 1, m), a, m)) % m;
        }

    }
}
