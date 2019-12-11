/***************************************************
 **类 名 称:ST_USER_DAL
 **命名空间:DCSoft.Demo.DAL
 **公司名称:定川信息
 **创建时间:2018/8/29 8:52:36
 **作    者:MJ
 **描    述:用户信息 DAL
 **修改时间:
 **修 改 人:
***********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DCSoft.DBUtilityGeneric.DAL;
using DCSoft.Demo.Model;
using Dos.ORM;

namespace DCSoft.Demo.DAL
{
    /// <summary>
    /// 用户信息 DAL
    /// </summary>
    public class ST_USER_DAL:BaseDAL<ST_USER>
    {
        /// <summary>
        /// 分页查询用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="pageSize">每页显示的数目</param>
        /// <param name="pageNumber">当前页码</param>
        /// <param name="cnt">满足条件的记录数</param>
        /// <returns></returns>
        public List<ST_USER> ListUsers(string userName, int pageSize, int pageNumber, out int cnt)
        {
            Where<ST_USER> where = new Where<ST_USER>();
            if (!string.IsNullOrEmpty(userName))
            {
                where.And(ST_USER._.userName.Like( userName));
            }

            //调用基类的方法查询
            cnt = getCount(where);
            return getPage(pageSize, pageNumber, where, ST_USER._.userName.Asc);

        }


    }
}
