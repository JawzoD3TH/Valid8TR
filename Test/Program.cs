using System;
using Valid8TR;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Valid8tr ValidationEngine = new Valid8tr();
            Console.WriteLine("-- Valid8TR Engine --");
            while (true)
                Console.WriteLine(ValidationEngine.Validate(Console.ReadLine()));

        }
    }
}
