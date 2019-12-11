/***************************************************
 **类 名 称:BusinessErrorException
 **命名空间:WebApiForDcSoft.WebAPIExtend.ExceptionHandle
 **公司名称:定川公司
 **创建时间:2018/3/13 15:54:05
 **作    者:my
 **描    述:业务系统自定义错误信息
 **修改时间:
 **修 改 人:
***********************************************/

using DCSoft.WebAPI.Utility.ExceptionHandle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using WebApiForDcSoft.WebAPIExtend.ExceptionModel;

namespace WebApiForDcSoft.WebAPIExtend.ExceptionHandle
{
    /// <summary>
    /// 业务系统自定义错误信息
    /// </summary>
    public class BusinessErrorException : CustomException
    {
        #region 初始化异常枚举
        /// <summary>
        /// 错误枚举文字描述信息
        /// </summary>
        static Dictionary<BusinessError, string> businessErrorExceptionDic;

        /// <summary>
        /// 初始化枚举值
        /// </summary>
        static BusinessErrorException()
        {
            businessErrorExceptionDic = new Dictionary<BusinessError, string>();

            //初始化枚举变量
            foreach (BusinessError value in Enum.GetValues(typeof(BusinessError)))
            {
                if (!businessErrorExceptionDic.ContainsKey(value))
                {
                    object[] objAttrs = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(BusinessError), true);

                    string discrption = "未定义错误码";
                    if (objAttrs != null && objAttrs.Length > 0)
                    {
                        DescriptionAttribute descAttr = objAttrs[0] as DescriptionAttribute;
                        discrption = descAttr.Description;
                    }
                    businessErrorExceptionDic.Add(value, discrption);
                }
            }
        }
        #endregion

        /// <summary>
        /// 错误代码
        /// </summary>
        private BusinessError exceptionCode;

        /// <summary>
        /// 业务系统自定义错误信息
        /// </summary>
        /// <param name="exceptionCode">错误代码</param>
        public BusinessErrorException(BusinessError exceptionCode) : base((int)exceptionCode)
        {
            this.exceptionCode = exceptionCode;
        }

        /// <summary>
        /// 错误代码描述属性
        /// </summary>
        public override string BusinessErrorMessage
        {
            get {
                if (businessErrorExceptionDic.ContainsKey(exceptionCode))
                    return businessErrorExceptionDic[exceptionCode];
                return exceptionCode.ToString();
            }
        }

    }
}