/***************************************************
 **类 名 称:UserTokenExtend
 **命名空间:DCSoft.WebAPI.WebAPIExtend.Authorise
 **公司名称:定川公司
 **创建时间:2018/7/4 8:45:46
 **作    者:my
 **描    述:用户Token扩展示例
 **修改时间:
 **修 改 人:
***********************************************/

using DCSoft.WebAPI.Utility.AuthorizeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCSoft.WebAPI.WebAPIExtend.Authorise
{
    /// <summary>
    /// 用户扩展示例
    /// </summary>
    public class UserTokenExtend:UserToken
    {
        /// <summary>
        /// 行政区划代码
        /// </summary>
       public string ADCD { get; set; }
    }
}