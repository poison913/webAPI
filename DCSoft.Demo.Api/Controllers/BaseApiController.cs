using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using DCSoft.WebAPI.WebAPIExtend.Authorise;
using DCSoft.Utility.Utils;
using DCSoft.Utility.Web;
using Newtonsoft.Json;

namespace DCSoft.Demo.Api.Controllers
{
    public class BaseApiController : ApiController
    {

        /// <summary>
        /// 当前用户请求信息
        /// </summary>
        public UserTokenExtend userToken { get; set; }


        #region 返回数据序列化为JSON

        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>HttpResponseMessage.</returns>
        protected HttpResponseMessage ToJson(ResponseMessage msg, JsonSerializerSettings settting = null)
        {
            string json = "";
            if (null == settting)
                json = msg.ToJson();
            else
                json = msg.ToJson(settting);

            HttpResponseMessage result = new HttpResponseMessage
            {
                Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json")
            };

            // result.StatusCode = HttpStatusCode.OK;
            return result;
        }

        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>HttpResponseMessage.</returns>
        protected HttpResponseMessage ToJson(Object obj)
        {
            String str;
            //if (obj is String || obj is Char)
            //{
            //    str = obj.ToString();
            //}
            //else
            {
                str = JsonHelper.Object2Json(obj);
            }
            HttpResponseMessage result = new HttpResponseMessage
            {
                Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            // result.StatusCode = HttpStatusCode.OK;

            return result;

        }

        /// <summary>
        /// 生成异常的响应消息
        /// </summary>
        /// <param name="errMsg">The MSG.</param>
        /// <returns>HttpResponseMessage.</returns>
        protected HttpResponseMessage GenernateErrorMsg(string errMsg)
        {
            ResponseMessage responseMsg = new ResponseMessage(-1);
            responseMsg.message = errMsg;

            string json = responseMsg.ToJson();
            HttpResponseMessage result = new HttpResponseMessage
            {
                Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json")
            };
            return result;
        }

        #endregion

        #region 获取常见类型的参数

        /// <summary>
        /// 从动态参数变量获取字典类型的参数
        /// </summary>
        /// <param name="param">动态参数</param>
        /// <returns></returns>
        [NonAction]
        protected Dictionary<string, object> GetDictionary(dynamic param)
        {
            Dictionary<string, object> dic = JsonHelper.Json2Dictionary(JsonHelper.Object2Json(param));
            if (dic == null)
                dic = new Dictionary<string, object>();
            return new Dictionary<string, object>(dic, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 根据key获取字典对象的值，key不存在返回空
        /// </summary>
        /// <param name="dicParams"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        protected string GetDictValue(Dictionary<string, object> dicParams, string key)
        {
            if (dicParams == null || !dicParams.ContainsKey(key) || dicParams[key] == null)
            {
                return "";
            }
            return dicParams[key].ToString().Trim();

        }

        /// <summary>
        /// 获取整数类型的参数
        /// </summary>
        /// <param name="dicParams"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        protected int? GetInt32(Dictionary<string, object> dicParams, string key)
        {
            int tmp = 0;
            if (null != dicParams && dicParams.ContainsKey(key) && int.TryParse(dicParams[key].ToString(), out tmp))
                return Convert.ToInt32(dicParams[key].ToString());
            return null;
        }

        /// <summary>
        /// 根据key获取字典对象的值,key不存在抛出异常
        /// </summary>
        /// <param name="dicParams"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        protected string GetDictValueMust(Dictionary<string, object> dicParams, string key)
        {
            if (dicParams == null || !dicParams.ContainsKey(key))
            {
                throw new Exception("缺少参数：" + key);
            }
            return GetDictValue(dicParams, key);

        }

        /// <summary>
        /// 根据key获取字典对象的值,key不存在或值为空抛出异常
        /// </summary>
        /// <param name="dicParams"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        protected string GetDictValueNotEmpty(Dictionary<string, object> dicParams, string key)
        {
            if (dicParams == null || !dicParams.ContainsKey(key))
            {
                throw new Exception("缺少参数：" + key);
            }
            string val = GetDictValue(dicParams, key);
            if (val == string.Empty)
            {
                throw new Exception("参数：" + key + " 不能为空");
            }
            return val;
        }

        /// <summary>
        /// 检查不能为空字段
        /// </summary>
        /// <param name="dicParams"></param>
        /// <param name="prefix"></param>
        /// <param name="listKey"></param>
        [NonAction]
        protected void CheckDictValueNotEmpty(Dictionary<string, object> dicParams, string prefix, params string[] listKey)
        {
            foreach (var key in listKey)
            {
                if (!dicParams.ContainsKey(key) || dicParams[key] == null || dicParams[key].ToString().Trim() == "")
                {
                    if (!string.IsNullOrEmpty(prefix))
                        throw new Exception("参数：" + prefix + "." + key + " 不能为空");
                    throw new Exception("参数：" + key + " 不能为空");
                }
            }
        }

        /// <summary>
        /// 获取时间类型的参数
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [NonAction]
        protected DateTime? GetDateTime(Dictionary<string, object> dict, string key)
        {
            DateTime tmp = DateTime.Now;
            if (null != dict && dict.ContainsKey(key) && null != dict[key] && DateTime.TryParse(dict[key].ToString(), out tmp))
                return Convert.ToDateTime(dict[key].ToString());
            return null;
        }

        /// <summary>
        /// 获取分页参数(输入参数为页码和页大小)
        /// </summary>
        /// <param name="dcParams"></param>
        /// <param name="iPageNumber">查询的页码（从1开始 ）</param>
        /// <param name="iPageSize">每页条目数</param>
        /// <returns></returns>
        [NonAction]
        protected void GetPageParms(Dictionary<string, object> dcParams, out int iPageNumber, out int iPageSize)
        {

            string strPageIndex = GetDictValue(dcParams, "pageNumber").Trim();
            string strPageSize = GetDictValue(dcParams, "pageSize").Trim();
            iPageNumber = Convert.ToInt32(string.IsNullOrEmpty(strPageIndex) ? "1" : strPageIndex);
            iPageSize = Convert.ToInt32(string.IsNullOrEmpty(strPageSize) ? "10" : strPageSize);

        }

        #endregion





    }
}
