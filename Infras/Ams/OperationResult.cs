using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ams
{
    /// <summary>
    /// 操作结果
    /// </summary>
    public class OperationResult
    {
        public OperationResult()
        {
            this.Result = ResultType.Success;
        }

        public ResultType Result { get; set; }

        public string Msg { get; set; }

        public object Data { get; set; }
    }

    /// <summary>
    /// 结果类型
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 错误
        /// </summary>
        Error = -1,

        /// <summary>
        /// 提醒
        /// </summary>
        Info = 0,

        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 失败
        /// </summary>
        Faild = 2,
    }

   
}
