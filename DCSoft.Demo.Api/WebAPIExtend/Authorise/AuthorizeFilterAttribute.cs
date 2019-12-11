/***************************************************
 **类 名 称:AuthFilterAttribute
 **命名空间:DCSoft.WebAPI.Utility.Authorise
 **公司名称:定川公司
 **创建时间:2018/3/4 16:17:29
 **作    者:my
 **描    述:webAPI权限验证
 **修改时间:
 **修 改 人:
***********************************************/
using DCSoft.Utility.Utils;
using DCSoft.Utility.Web;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace DCSoft.WebAPI.Utility.Authorise
{
    /// <summary>
    /// 验证处理函数
    /// </summary>
    /// <param name="actionContext"></param>
    /// <returns></returns>
    public delegate  bool ValidateHandler(HttpActionContext actionContext);
    /// <summary>
    /// 权限验证属性，在需要添加权限验证的类名或方法名上添加该属性即可
    /// </summary>
    public class AuthorizeFilterAttribute : AuthorizeAttribute
    {
        public static ValidateHandler VilidataHandler;

        /// <summary>
        /// 重写基类的验证模式，以我们自己的方式实现验证
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (VilidataHandler != null)
            {
                //获取Controller是否允许匿名访问
                //var controllerAttributes = actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                //bool isControllerAnonymous = controllerAttributes.Any(a => a is AllowAnonymousAttribute);

                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                //若验证方法为空，或未通过验证，则判断用户访问方法是否可以
                if (!isAnonymous&&!VilidataHandler(actionContext))
                {
                    if (isAnonymous)
                    {
                        base.OnAuthorization(actionContext);
                    }
                    else
                    {
                        //返回未验证错误
                        actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized)
                        {
                            Content = new StringContent(JsonHelper.Object2Json(new ResponseMessage(401001, "登录验证失败")), Encoding.UTF8, "application/json"),
                            ReasonPhrase = "Exception"
                        };
                        //HandleUnauthorizedRequest(actionContext);
                    }
                }
                else
                {
                    base.IsAuthorized(actionContext);
                }
            }
        }       
    }
}
