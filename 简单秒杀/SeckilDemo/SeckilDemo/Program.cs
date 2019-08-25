using System;
using System.Threading.Tasks;

namespace SeckilDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var start = DateTime.Now;
            ThreadBuy();
            var end = DateTime.Now;
            Console.WriteLine("------操作完成--------");
            Console.WriteLine($"执行时间:{end - start}");
            Console.ReadKey();
            //Console.WriteLine("Hello World!");
        }


        //秒杀
        static void ThreadBuy()
        {
            System.Threading.Thread.Sleep(2 * 1000);
           var tt = new ThreadPar();

            #region for循环
            //for(var i = 0; i < 1000000; i++)
            //{
            //    var uid= "用户" + i;
            //    var x = tt.Buy(uid);

            //    if (x.Code == -1)
            //    {
            //        Console.WriteLine(uid + ":" + x.Msg);
            //    }
            //    else
            //    {
            //        Console.WriteLine(uid + ":" + x.Msg + "还剩下" + x.Cnt + "件");
            //    }
            //}

            #endregion

            ParallelOptions options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 4   //最大三个线程
            };

            //Parallel.For(0, 1000000, options, (t, state) => 
             Parallel.For(0, 1000000,(t, state) =>

            {
                var uid = "用户" + t;
                var x = tt.Buy(uid);

                if (x.Code == -1)
                {
                    Console.WriteLine(uid + ":" + x.Msg);
                }
                else
                {
                    Console.WriteLine(uid + ":" + x.Msg + "还剩下" + x.Cnt + "件");
                }

            });

//            tt.Dispose();   //销毁
        }

    }
}
