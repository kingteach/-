using Ams;
using Ams.Security;
using Ams.Utils;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
 

namespace Ams.Components.Security
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserMgr : IUserService
    {
        public IUser GetUserInfo()
        {
            throw new NotImplementedException();
        }

        //private const string CLEARTEXT = "soft";

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public OperationResult ChangePwd(string userId, string oldPwd, string newPwd)
        {
            if (oldPwd == newPwd) return new OperationResult() { Result = ResultType.Info, Msg = "原密码和新密码一样" };

            if (StringUtil.HasText(oldPwd) && StringUtil.HasText(newPwd))
            {
                try
                {
                    string pwd = EncryptUtil.Encrypt(newPwd);

                    using (EHRPEntities ctx = new EHRPEntities())
                    {
                        var userInfo = ctx.LC_USER.FirstOrDefault(x => x.PK == userId);
                        if (userInfo == null) return new OperationResult() { Result = ResultType.Faild, Msg = "未找到相关用户" };
                        userInfo.PWD = pwd;
                        ctx.SaveChanges();
                        return new OperationResult() { Msg = "修改成功" };
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("修改密码异常：" + ex.Message);
                }

            }
            else
            {
                return new OperationResult() { Result = ResultType.Faild, Msg = "密码不能为空" };
            }
        }

        /// <summary>
        /// 验证原密码是否正确
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPwd"></param>
        /// <returns></returns>
        public bool CheckOldPwd(string userId, string oldPwd)
        {
            string pwd = EncryptUtil.Encrypt(oldPwd);
            using (EHRPEntities ctx = new EHRPEntities())
            {
                return ctx.LC_USER.FirstOrDefault(x => x.PK == userId && x.PWD == pwd) != null;
            }
        }
        #endregion

        #region 登录、退出


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns>Data:1:表示用户名有问题，2:表示密码有问题，0;表示公共问题，对象：表示登录用户</returns>
        public OperationResult Login(IUser userInfo)
        {
            if (string.IsNullOrEmpty(userInfo.UserName) || string.IsNullOrEmpty(userInfo.Pwd))
            {
                return new OperationResult() { Result = ResultType.Faild, Msg = "用户名或密码不能为空", Data = 0 };
            }
            using (AMSEntities ctx = new AMSEntities())
            {
                var user = (from p in ctx.LC_USER
                            where p.ACCOUNT == userInfo.UserName
                            select new UserInfo()
                            {
                                Id = p.PK,
                                UserName = p.ACCOUNT,
                                RealName = p.REALNAME,
                                Pwd = p.PWD,
                                IsValid = p.ISVALID == 1,
                                DetailId = p.CSPK,
                            }).FirstOrDefault();

                if (user == null)
                {
                    return new OperationResult() { Result = ResultType.Faild, Msg = "用户名不存在，请重新输入", Data = 1 };
                }
                else if (!user.IsValid)
                {
                    return new OperationResult() { Result = ResultType.Faild, Msg = "用户未启用，请联系管理员", Data = 0 };
                }
                else if (user.Pwd != EncryptUtil.Encrypt(userInfo.Pwd))
                {
                    return new OperationResult() { Result = ResultType.Faild, Msg = "用户名与密码不匹配，请重新输入", Data = 2 };
                }
                return new OperationResult() { Result = ResultType.Success, Data = user };
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public bool LoginOut()
        {
            FormsAuthentication.SignOut();
            return true;
        }
        #endregion

        #region 忘记密码

        public OperationResult GeneralValidActiveCode(string email)
        {
            try
            {
                string vCode = StringUtil.Guid().Replace("-", "").ToLower();
                string vTime = EncryptUtil.Encrypt(DateTime.Now.ToString());

                using (EHRPEntities ctx = new EHRPEntities())
                {
                    var userInfo = ctx.LC_USER.FirstOrDefault(x => x.EMAIL == email);
                    if (userInfo == null) return new OperationResult() { Result = ResultType.Faild, Msg = "邮箱不存在" };
                    userInfo.VCODE = vCode;
                    userInfo.VTIME = vTime;
                    ctx.SaveChanges();
                    return new OperationResult() { Data = new Tuple<string, string, string>(email, vCode, vTime) };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("通过邮箱从数据库获取验证激活码失败：" + ex.Message);
            }
        }

        public bool CheckValidActiveCode(string email, string vCode, string vTime)
        {
            try
            {
                using (EHRPEntities ctx = new EHRPEntities())
                {
                    var userInfo = ctx.LC_USER.FirstOrDefault(x => x.EMAIL == email && x.VCODE == vCode && x.VTIME.Trim() == vTime.Trim());
                    return userInfo != null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("验证激活码失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public OperationResult ResetPwdByEmail(string email, string newPwd)
        {
            try
            {
                string pwd = EncryptUtil.Encrypt(newPwd);
                using (EHRPEntities ctx = new EHRPEntities())
                {
                    var userInfo = ctx.LC_USER.FirstOrDefault(x => x.EMAIL == email);
                    if (userInfo == null) return new OperationResult() { Result = ResultType.Faild, Msg = "邮箱不存在" };
                    userInfo.PWD = pwd;
                    userInfo.VCODE = null;
                    userInfo.VTIME = null;
                    ctx.SaveChanges();
                    return new OperationResult() { Msg = "重置密码成功" };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("通过邮箱重置密码失败：" + ex.Message);
            }
        }


        #endregion
    }
}
