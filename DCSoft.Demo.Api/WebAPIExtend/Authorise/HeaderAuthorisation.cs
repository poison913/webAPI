/***************************************************
 **类 名 称:HeaderAuthorisation
 **命名空间:DCSoft.WebAPI.WebAPIExtend.Authorise
 **公司名称:定川公司
 **创建时间:2018/3/6 10:31:33
 **作    者:my
 **描    述:Header头部验证
 **修改时间:
 **修 改 人:
***********************************************/

using DCSoft.WebAPI.Utility.AuthorizeModel;
using System;
using System.Web;
using System.Web.Http.Controllers;

namespace DCSoft.WebAPI.WebAPIExtend.Authorise
{
    /// <summary>
    /// 实现权限验证实际实现方法
    /// </summary>
    public static class HeaderAuthorisation
    {
        //TODO:具体的权限实现方法
        /// <summary>
        /// 权限验证方法
        /// </summary>
        /// <param name="actionContext">Web请求</param>
        /// <returns>返回是否通过验证</returns>
        public static bool ValidateHandler(HttpActionContext actionContext)
        {
            //var token = HttpContext.Current.Request.Headers["Token"];//actionContext.Request.Headers.["Token"];
            //if (string.IsNullOrEmpty(token))
            //    return false;

            //var isvalid = UserTokenManager.ValidUserToken(token);
            //return isvalid;


            var token = HttpContext.Current.Request.QueryString["token"] ?? null;
            if (string.IsNullOrEmpty(token))
            {
                token = HttpContext.Current.Request.Headers["token"];
                if (string.IsNullOrEmpty(token))
                    return false;
            }

            IHttpController controller = actionContext.ControllerContext.Controller;
            if (controller is Demo.Api.Controllers.BaseApiController)
            {
                UserTokenExtend userToken = UserTokenManager.GetUserToken(token) as UserTokenExtend;
                if (null == userToken)
                {
                    //token已过期
                    return false;
                }
                //测试使用的token
                if ("dcxx123456" != token)
                    userToken.ExpireTime = DateTime.Now.AddHours(3);
                else userToken.ExpireTime = DateTime.Now.AddYears(1);
                ((DCSoft.Demo.Api.Controllers.BaseApiController)controller).userToken = userToken;
            }

            return true;

        }
    }
}