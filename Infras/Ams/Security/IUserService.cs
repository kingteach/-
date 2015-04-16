using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ams.Security
{
    public interface IUserService
    {
        IUser GetUserInfo();

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        OperationResult ChangePwd(string userId, string oldPwd, string newPwd);

        /// <summary>
        /// 验证老密码是否正确
        /// </summary>
        /// <param name="oldPwd"></param>
        /// <returns></returns>
        bool CheckOldPwd(string userId, string oldPwd);
        #endregion

        #region 登录、退出
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        OperationResult Login(IUser userInfo);

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        bool LoginOut();
        #endregion

        #region 忘记密码

        /// <summary>
        /// 生成重置密码需要的验证激活码
        /// </summary>
        /// <param name="email"></param>
        /// <returns>邮箱and验证码and验证时间</returns>
        OperationResult GeneralValidActiveCode(string email);

        /// <summary>
        /// 验证 激活码
        /// </summary>
        /// <param name="email"></param>
        /// <param name="vCode"></param>
        /// <param name="vTime"></param>
        /// <returns></returns>
        bool CheckValidActiveCode(string email, string vCode, string vTime);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="email"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        OperationResult ResetPwdByEmail(string email, string newPwd);

        #endregion
    }
}
