using System;
using Topshelf;

namespace Framework.QuartzConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<QuartzServer>();
                x.SetDescription("QuartzServer");
                x.SetDisplayName("QuartzServer");
                x.SetServiceName("QuartzServer");
                x.EnablePauseAndContinue();
                x.RunAsLocalSystem();
            });
            Console.ReadKey();
        }
    }
}
