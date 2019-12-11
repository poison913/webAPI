using DCSoft.Utility.Utils;
using DCSoft.Utility.Web;
using DCSoft.WebAPI.Utility.Authorise;
using DCSoft.WebAPI.Utility.AuthorizeModel;
using DCSoft.WebAPI.WebAPIExtend.Authorise;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using WebApiForDcSoft.WebAPIExtend.Authorise;

namespace DCSoft.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 应用程序启动配置
        /// </summary>
        protected void Application_Start()
        {
            //TODO:设置log4net配置文件读取
            var logCfg = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));
            XmlConfigurator.ConfigureAndWatch(logCfg);

            //TODO:配置权限验证实现方法
            AuthorizeFilterAttribute.VilidataHandler += HeaderAuthorisation.ValidateHandler;

            //TODO:设置权限验证Token的存取实现方法
            UserTokenManager.TokenRep = new UserTokenRepository();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //TODO   添加测试使用的默认token信息

            LogHelper.LogInfoMsg("--------应用程序启动--------", MethodBase.GetCurrentMethod().Name);

        }

        /// <summary>
        /// 重写webAPI配置
        /// </summary>
        public override void Init()
        {
            //TODO:启用Session,若不需要注释
            this.PostAuthenticateRequest += (sender, e) => HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);

            base.Init();
        }



        /// <summary>
        /// 应用程序出错
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码
            Exception ex = Server.GetLastError().GetBaseException();
            Server.ClearError();
            if (ex.GetType() == typeof(HttpException))
            {
                HttpException httpEx = (HttpException)ex;
                LogHelper.LogErrorMsg(ex, "--------应用程序出错！--------" + httpEx.GetHtmlErrorMessage());
            }
            else
            {
                LogHelper.LogErrorMsg(ex, "--------应用程序出错！--------" + ex.StackTrace);
            }

            //ResponseMessage msg = new ResponseMessage(-1);
            //string errorMsg = ex.Message;
            //msg.message = errorMsg;
            //Context.Response.AddHeader("Content-type", "text/html;charset=UTF-8");

            //if (errorMsg.Contains("未找到"))
            //    Context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            //else
            //    Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //Context.Response.Write(msg.ToJson());

        }



        /// <summary>
        /// Application_End
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End(object sender, EventArgs e)
        {
            LogHelper.LogInfoMsg("--------应用程序结束--------", MethodBase.GetCurrentMethod().Name);
        }




    }
}
