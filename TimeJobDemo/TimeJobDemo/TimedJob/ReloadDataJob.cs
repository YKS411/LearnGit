using Pomelo.AspNetCore.TimedJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeJobDemo.Model;

namespace TimeJobDemo.TimedJob
{
    public class ReloadDataJob:Job
    {

        /// <summary>
        /// Begin:起始时间；Interval:执行时间间隔，单位是毫秒；SkipWhileExexuting是否等待上一个执行完成，true为等待
        /// </summary>
        [Invoke(Begin="2019-07-30 22:40",Interval =1000*10,SkipWhileExecuting =true)]
        public void ReloadData()
        {
            var temp1 = new Shop {Name="香蕉",price=3.8M,amount=100 };
            var temp2= new Shop { Name = "苹果", price = 9.8M, amount = 100 };
            var temp3= new Shop { Name = "荔枝", price =8.5M, amount = 100 };
            var dataList = new List<Shop>();
            dataList.Add(temp1);
            dataList.Add(temp2);
            dataList.Add(temp3);
            Helper.SetShopsList(dataList);

        }
    }
}
