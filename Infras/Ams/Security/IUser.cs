using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ams.Security
{
    public interface IUser
    {
        decimal Xh { get; set; }

        string Id { get; set; }
        string UserName { get; set; }
        string RealName { get; set; }

        string Pwd { get; set; }

        bool RememberMe { get; set; }

        /// <summary>
        /// 详细信息的Id
        /// </summary>
        string DetailId { get; set; }

        bool IsValid { get; set; }
    }
}
