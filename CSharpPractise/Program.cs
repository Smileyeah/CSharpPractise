using System;
using System.Threading;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace CSharpPractise
{
    class Program
    {
        private static readonly string[] InnerIpAddressMatchers = new[]
        {
            @"^10\.(1\d{2}|2[0-4]\d|25[0-5]|[1-9]\d|[0-9])\.(1\d{2}|2[0-4]\d|25[0-5]|[1-9]\d|[0-9])\.(1\d{2}|2[0-4]\d|25[0-5]|[1-9]\d|[0-9])$",
            @"^172\.(1[6789]|2[0-9]|3[01])\.(1\d{2}|2[0-4]\d|25[0-5]|[1-9]\d|[0-9])\.(1\d{2}|2[0-4]\d|25[0-5]|[1-9]\d|[0-9])$",
            @"^192\.168\.(1\d{2}|2[0-4]\d|25[0-5]|[1-9]\d|[0-9])\.(1\d{2}|2[0-4]\d|25[0-5]|[1-9]\d|[0-9])$"
        };
        
        static void Main(string[] args)
        {
            var inner = IsInnerIpAddress("172.16.0.4");
            var inner1 = IsInnerIpAddress("122.9.126.14");
            
            int x = 1;
            int y = 10;

            var @base = Math.Pow(2, 16) ;

            Console.WriteLine(@base);

            var result = CalculatorA(x, y);

            Console.WriteLine(result);

            Console.ReadKey();

        }

        private static int CalculatorA(int x, int y)
        {
            if (y == 0)
                return 0;

            if (x == 0)
                return 2 * y;

            if (y == 1)
                return 2;

            return CalculatorA(x - 1, CalculatorA(x, y - 1));
        }

        private static bool IsInnerIpAddress(string ipAddress)
        {
            foreach (var matcher in InnerIpAddressMatchers)
            {
                if (Regex.IsMatch(ipAddress, matcher))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
