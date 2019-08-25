using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SeckilDemo
{
    public class ThreadPar
    {
        private static int cnt = 10;
        private static Queue queryAll = new Queue();
        private static Queue<string> queryCur = new Queue<string>();
        //包装同步---线程安全
        Queue mySyncdQ = Queue.Synchronized(queryAll);


        //使用并发队列
        private static ConcurrentQueue<string> cqueryAll = new ConcurrentQueue<string>();
        private static ConcurrentQueue<string> cqueryCur = new ConcurrentQueue<string>();


        static ThreadPar()
        {
            HandleQueue();
        }

         public RetData Buy(string uid)
        {
            #region 同步
            mySyncdQ.Enqueue(uid);
            if (mySyncdQ.Count > cnt)

            //queryAll.Enqueue(uid);
            //if (queryAll.Count > cnt)
            {
                return new RetData { Code = -1, Msg = "商品抢光了", Cnt = 0 };
            }
            queryCur.Enqueue(uid);
            return new RetData { Code = 0, Msg = "恭喜已抢到", Cnt = cnt - queryAll.Count };
            #endregion

            #region 并发
            //cqueryAll.Enqueue(uid);
            //if (cqueryAll.Count > cnt)
            //{
            //    return new RetData { Code = -1, Msg = "商品抢光了", Cnt = 0 };
            //}
            //cqueryCur.Enqueue(uid);
            //return new RetData { Code = 0, Msg = "恭喜已抢到", Cnt = cnt - queryAll.Count };
            #endregion
        }


        public class RetData
        {
            public int Code { get; set; }
            public string Msg { get; set; }
            public int Cnt { get; set; }
        }

        public static void HandleQueue()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                    if (queryCur.Count > 0)
                        HandleOrder();
            });


            //并发
            //Task.Factory.StartNew(() =>
            //{
            //    while (true)
            //        if (cqueryCur.Count > 0)
            //            HandleOrder();
            //});
        }


        public static void HandleOrder()
        {
            while (queryCur.Count != 0)
            {
                Console.WriteLine("处理用户订单中：" + queryCur.Dequeue());
            }

            //while (cqueryCur.Count != 0)
            //{
            //    string id;
            //    Console.WriteLine("处理用户订单中：" + cqueryCur.TryDequeue(out id));
            //}
        }


        public void Dispose()
        {
                queryAll.Dequeue();        
        }

    }
}
