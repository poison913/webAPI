using DCSoft.WebAPI.Utility.Authorise;
using DCSoft.WebAPI.Utility.AuthorizeModel;
using DCSoft.WebAPI.WebAPIExtend.Authorise;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}
