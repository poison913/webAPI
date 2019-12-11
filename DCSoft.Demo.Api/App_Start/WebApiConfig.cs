using DCSoft.WebAPI.Utility.APISetter;
using DCSoft.WebAPI.Utility.Authorise;
using DCSoft.WebAPI.Utility.ExceptionHandle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DCSoft.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // TODO:Web API 配置和服务
            // 配置Web API仅使用application/json 字符串返回，并设置返回时间格式及空字符串格式
            InitAPI.SetWebAPIReturnJson(config);

            // Web API 路由
            config.MapHttpAttributeRoutes();

            //TODO:修改web API路由
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //TODO:全局添加WebAPI 统一错误处理，这种只会捕获WebAPI方法中的未捕获异常
            config.Filters.Add(new WebApiExceptionFilterAttribute());

            //TODO:为地址不对添加异常记录
            config.MessageHandlers.Add(new CustomErrorMessageDelegatingHandler());

            //TODO:添加全局权限验证
            config.Filters.Add(new AuthorizeFilterAttribute());

        }
    }
}

 

