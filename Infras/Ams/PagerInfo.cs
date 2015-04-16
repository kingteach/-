using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ams
{
    /// <summary>
    /// 分页信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagerInfo<T> where T : class
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string OrderBy { get; set; }

        public bool IsAsc { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        /// <summary>
        /// 分页查询结果
        /// </summary>
        public List<T> Data { get; set; }


    }
}
