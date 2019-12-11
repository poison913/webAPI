/***************************************************
 **类 名 称:EnumError
 **命名空间:WebApiForDcSoft.WebAPIExtend.ExceptionModel
 **公司名称:定川公司
 **创建时间:2018/3/13 15:32:45
 **作    者:my
 **描    述:错误枚举自定义范围
 **修改时间:
 **修 改 人:
***********************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApiForDcSoft.WebAPIExtend.ExceptionModel
{
    /// <summary>
    /// 错误自定义枚举
    /// TODO:增加枚举值，添加自定义错误
    /// </summary>
    public enum BusinessError : int
    {
        /// <summary>
        /// 0 预留值，成功
        /// </summary>
        [Description("成功")]
        Success = 0,

        #region 验证错误
        /// <summary>
        /// 参数类型错误
        /// </summary>
        [Description("成功")]
        InvalidType,
        /// <summary>
        /// 非法参数
        /// </summary>
        [Description("非法参数")]
        InvalidParameter,
        /// <summary>
        /// 非法数字
        /// </summary>
        [Description("非法数字")]
        InvalidNumber,
        #endregion

        #region 权限验证
        /// <summary>
        /// 非法请求
        /// </summary>
        [Description("非法请求")]
        InvalidRequest,
        /// <summary>
        /// 用户密码有误
        /// </summary>
        [Description("用户密码有误")]
        InvalidPassword,
        /// <summary>
        /// 用户名错误
        /// </summary>
        [Description("用户名错误")]
        InvalidUserName,

        /// <summary>
        /// 非法的Token值
        /// </summary>
        [Description("非法的Token值")]
        InvalidToken
        #endregion

        //TODO:增加自定义错误枚举值及描述

    }
}