using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
 


namespace  Ams
{
    /// <summary>
    /// 系统全局变量
    /// </summary>
    public class Global
    {
        private static readonly Global instance = new Global();
        public static Global Instance
        {
            get { return instance; }
        }
      
        #region 读配置
        public int PageSize { get { return 10; } }
        #endregion

        #region 常量定义
        public const string PLEASECHOOSE = "请选择";
        public const string PLEASECHOOSE_VAL = "";

        public const string CHOOSEALL = "全部";

        /// <summary>
        /// 启用标志
        /// </summary>
        public const string QYBZ = "0";
        ///// <summary>
        ///// 操作员号
        ///// </summary>
        //public const string CZYH = "00";

        //public const string CZYM = "supervisor";
        #endregion

        #region 图片
        /// <summary>
        ///（暂无图片）地址
        /// </summary>
        public string NoImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(_noImagePath))
                {
                    _noImagePath = "~/Content/images/noimg.png";
                }
                return _noImagePath;
            }
        }
        private string _noImagePath;

        /// <summary>
        /// 资质证件图片临时存放地址
        /// </summary>
        public string CertPicTempPath
        {
            get
            {
                if (string.IsNullOrEmpty(_certPicTempPath))
                {
                    _certPicTempPath = "~/Content/temp/Cert";
                }
                return _certPicTempPath;
            }
        }
        private string _certPicTempPath;

        /// <summary>
        /// 物资规格图片临时存放地址
        /// </summary>
        public string SpecPicTempPath
        {
            get
            {
                if (string.IsNullOrEmpty(_specPicTempPath))
                {
                    _specPicTempPath = "~/Content/temp/Spec";
                }
                return _specPicTempPath;
            }
        }
        private string _specPicTempPath;
        #endregion
    }
}
