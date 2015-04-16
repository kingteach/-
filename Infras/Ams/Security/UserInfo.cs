using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ams.Security
{
    public class UserInfo : IUser
    {
        public decimal Xh
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }

        [Required(ErrorMessage = "请输入用户名")]
        public string UserName
        {
            get;
            set;
        }

        [Required(ErrorMessage = "请输入密码")]
        [DataType(DataType.Password)]
        public string Pwd { get; set; }

        [Required(ErrorMessage = "请输入验证码")]
        public string ValidCode { get; set; }


        public string RealName
        {
            get;
            set;
        }

        public bool RememberMe
        {
            get;
            set;
        }

        /// <summary>
        /// 详细信息的Id
        /// </summary>
        public string DetailId { get; set; }



        public bool IsValid { get; set; }

    }
}
