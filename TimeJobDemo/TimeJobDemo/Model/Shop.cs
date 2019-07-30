using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeJobDemo.Model
{
    public class Shop
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public uint amount { get; set; }
    }
}
