using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using DCSoft.Utility.Utils;
using DCSoft.Utility.Web;
using DCSoft.WebAPI.Utility.AuthorizeModel;
using DCSoft.WebAPI.WebAPIExtend.Authorise;

namespace DCSoft.Demo.Api.Controllers
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginController : BaseApiController
    {

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Login(dynamic param)
        {
            if (null == param)
            {
                return GenernateErrorMsg("请求参数不能为空!");
            }

            ResponseMessage responseMsg = new ResponseMessage(0);
            try
            {
                //获取字典类型的参数
                Dictionary<string, object> dictParams = GetDictionary(param);
                //用户名
                string userName = GetDictValueNotEmpty(dictParams, "userName");
                //密码
                string passWord = GetDictValueNotEmpty(dictParams, "passWord");


                #region 登录验证

                //TODO 

                #endregion

                #region 生成token

			    //TODO 传递用户参数信息，生成token
                UserTokenExtend myToken = GenernateToken();
                //设置当前登录用户信息
                this.userToken = myToken;
                LogHelper.DebugInfoMsg("用户登录：token=" + myToken.Token + " ; userName=", MethodBase.GetCurrentMethod().Name);

                #endregion

                //返回用户信息，根据需要返回相应的用户信息
                responseMsg.message = new { token = myToken.Token, expireTime = myToken.ExpireTime };
            }
            catch (Exception ex)
            {
                responseMsg.status = -1;
                responseMsg.message = ex.Message;
                LogHelper.LogErrorMsg(ex, "用户登录出错！");
            }

            return ToJson(responseMsg);
        }


        /// <summary>
        /// 生成token
        /// </summary>
        /// <returns></returns>
        private UserTokenExtend GenernateToken()
        {
            //保存用户访问标识token
            UserTokenExtend myToken = new UserTokenExtend();
            //时间戳
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 8, 0, 0, 0));
            long t = (DateTime.Now.Ticks - startTime.Ticks) / 10000; //除10000调整为13位    

            //时间戳+随机码
            string tokenStr = t + Guid.NewGuid().ToString().Replace("-", "").ToLower();

            myToken.Token = tokenStr;
            myToken.UserId = "";
            myToken.TrueName = "";
            //myToken.adcd = "";
            //myToken.userName = "";
            myToken.IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] ?? null;
            myToken.ExpireTime = DateTime.Now.AddHours(3);

            //保存token
            UserTokenManager.AddToken(myToken);

            return myToken;
        }





    }
}
