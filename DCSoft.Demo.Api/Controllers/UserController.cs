using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Http;
using DCSoft.Demo.DAL;
using DCSoft.Demo.Model;
using DCSoft.Utility.Utils;
using DCSoft.Utility.Web;

namespace DCSoft.Demo.Api.Controllers
{
    /// <summary>
    /// 用户接口控制器
    /// </summary>
    public class UserController : BaseApiController
    {
        /// <summary>
        /// 查询系统用户信息列表
        /// token: 用户标识
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage List(dynamic param)
        {
            if (null == param)
            {
                return GenernateErrorMsg("请求参数不能为空!");
            }
            ResponseMessage responseMsg = new ResponseMessage(0);
            try
            {
                #region 参数解析

                Dictionary<string, object> dcictParams = GetDictionary(param);
                //用户名
                string userName = GetDictValue(dcictParams, "userName");

                int pageSize = 10;
                int pageNumber = 1;
                GetPageParms(dcictParams, out pageNumber, out pageSize);

                #endregion

                ST_USER_DAL dal = new ST_USER_DAL();
                int cnt = 0;
                List<ST_USER> list = dal.ListUsers(userName, pageSize, pageNumber, out cnt);
                //输出分页格式的json数据
                responseMsg.message = new { total = cnt, rows = list };
            }
            catch (Exception ex)
            {
                responseMsg.status = -1;
                responseMsg.message = ex.Message;
                LogHelper.LogErrorMsg(ex, MethodBase.GetCurrentMethod().Name);
            }
            return ToJson(responseMsg);

        }
        /// <summary>
        /// 获取用户信息详情
        /// token: 用户标识
        /// 
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Get(dynamic param)
        {
            ResponseMessage responseMsg = new ResponseMessage(0);
            try
            {
                Dictionary<string, object> dictParams = GetDictionary(param);
                string userName = GetDictValue(dictParams, "userName");
                if (string.IsNullOrEmpty(userName))
                {
                    return GenernateErrorMsg("用户名不能为空!");
                }


                #region

                ST_USER_DAL dal = new ST_USER_DAL();
                ST_USER model = dal.getEntity(userName);
                responseMsg.message = model;

                #endregion
            }
            catch (Exception ex)
            {
                responseMsg.status = -1;
                responseMsg.message = ex.Message;
                LogHelper.LogErrorMsg(ex, MethodBase.GetCurrentMethod().Name);
            }

            return ToJson(responseMsg);
        }


        /// <summary>
        /// 添加用户信息信息
        /// token: 用户标识
        /// 
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Add(dynamic param)
        {
            ResponseMessage responseMsg = new ResponseMessage(0);
            if (null == param)
            {
                return GenernateErrorMsg("请求参数不能为空!");
            }

            try
            {
                Dictionary<string, object> dictParams = GetDictionary(param);

                //TODO  必填字段判断，其他字段略
                string userName = GetDictValueNotEmpty(dictParams, "userName");

                //系统用户信息
                ST_USER model = JsonHelper.Json2Object<ST_USER>(JsonHelper.Object2Json(dictParams));

                ST_USER_DAL dal = new ST_USER_DAL();

                //查询是否已经有对应名称的用户
                //注意，此处userName为主键，因此直接使用getEntity
                ST_USER oldEntity = dal.getEntity(userName.Trim());
                if (null != oldEntity)
                {
                    return GenernateErrorMsg("该用户名已存在!");
                }

                //TODO  其他信息校验

                #region

                //添加用户信息，返回影响的行数
                int cnt = dal.add(model);
                responseMsg.message = cnt;

                #region  添加日志记录

                //TODO  添加操作日志

                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                responseMsg.status = -1;
                responseMsg.message = ex.Message;
                LogHelper.LogErrorMsg(ex, MethodBase.GetCurrentMethod().Name);

            }

            return ToJson(responseMsg);
        }

        /// <summary>
        /// 更新用户信息
        /// token: 用户标识
        /// 
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Update(dynamic param)
        {
            ResponseMessage responseMsg = new ResponseMessage(0);
            if (null == param)
            {
                return GenernateErrorMsg("请求参数不能为空!");
            }

            try
            {
                Dictionary<string, object> dictParams = GetDictionary(param);
                //用户信息
                ST_USER model = JsonHelper.Json2Object<ST_USER>(JsonHelper.Object2Json(param));

                //TODO  必填字段判断，其他字段略
                string userName = GetDictValueNotEmpty(dictParams, "userName");

                ST_USER_DAL dal = new ST_USER_DAL();
                ST_USER oldEntity = dal.getEntity(userName);

                if (null == oldEntity)
                {
                    return GenernateErrorMsg("要更新的记录不存在!");
                }

                //TODO 其他字段校验

                //更新用户信息，返回影响的行数
                responseMsg.message = dal.update(model);

                #region

                #region 添加日志记录

                //TODO  添加操作日志

                #endregion

                #endregion

            }
            catch (Exception ex)
            {
                responseMsg.status = -1;
                responseMsg.message = ex.Message;
                LogHelper.LogErrorMsg(ex, MethodBase.GetCurrentMethod().Name);

            }
            return ToJson(responseMsg);
        }

        /// <summary>
        /// 删除用户信息
        /// token: 用户标识
        /// 
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Delete(dynamic param)
        {
            ResponseMessage responseMsg = new ResponseMessage(0);
            if (null == param)
            {
                return GenernateErrorMsg("请求参数不能为空!");
            }

            try
            {
                Dictionary<string, object> dictParams = GetDictionary(param);
                string userName = GetDictValueNotEmpty(dictParams, "userName");

                ST_USER_DAL dal = new ST_USER_DAL();
                ST_USER oldEntity = dal.getEntity(userName);

                if (null == oldEntity)
                {
                    return GenernateErrorMsg("要删除的记录不存在!");
                }

                //TODO  根据实际需要删除，一般做假删除操作，如果是真删除，需要删除与之相应的其他关联信息（此时需要使用事物操作）

                //删除用户信息，返回影响的行数
                responseMsg.message = dal.delete(userName);

                #region

                #region 添加日志记录

                //TODO  添加操作日志

                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                responseMsg.status = -1;
                responseMsg.message = ex.Message;
                LogHelper.LogErrorMsg(ex, MethodBase.GetCurrentMethod().Name);
            }

            return ToJson(responseMsg);
        }



    }
}
