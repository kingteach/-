using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewModel
{
    /// <summary>
    /// 单据审核状态
    /// </summary>
    public enum Statu
    {
        /// <summary>
        /// 已保存
        /// </summary>
        Saved = 0,

        /// <summary>
        /// 已提交（不可再修改） 待审核
        /// </summary>
        Submitted = 1,

        /// <summary>
        /// 审核通过（多个人审核时）  可以归档
        /// </summary>
        Approval = 2,

        /// <summary>
        /// 归档
        /// </summary>
        Filed = 3,

        /// <summary>
        /// 退回
        /// </summary>
        Back = 4,
    }

    public static class ext
    {
        public static string GetDesc(this Statu statu)
        {
            switch (statu)
            {
                case Statu.Saved:
                    return "已保存";
                case Statu.Submitted:
                    return "已提交";
                case Statu.Approval:
                    return "审核通过";
                case Statu.Filed:
                    return "归档";
                case Statu.Back:
                    return "退回";
            }
            return "未知";
        }
    }
}
