using System;

namespace SealDemo356
{
    class Program
    {
        static void Main(string[] args)
        {
            Example.EasyExample();

            GC.Collect();
        }
    }
}
