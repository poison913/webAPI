/***************************************************
 **类 名 称:UserTokenRepository
 **命名空间:DCSoft.WebAPI.Utility.AuthorizeModel
 **公司名称:定川公司
 **创建时间:2018/3/14 13:32:56
 **作    者:my
 **描    述:
 **修改时间:
 **修 改 人:
***********************************************/
using DCSoft.WebAPI.Utility.AuthorizeModel;
using System;
using System.Collections.Generic;
using WebApiForDcSoft.WebAPIExtend.ExceptionHandle;
using WebApiForDcSoft.WebAPIExtend.ExceptionModel;

namespace WebApiForDcSoft.WebAPIExtend.Authorise
{
    //TODO:根据项目的实际需要，设置token的存取方式
    /// <summary>
    /// Token存取方式
    /// </summary>
    public class UserTokenRepository : IUserTokenRepository
    {
        /// <summary>
        /// 用户token
        /// </summary>
        private static volatile Dictionary<string, UserToken> dictUserToken;
        /// <summary>
        /// 用户token
        /// </summary>
        private static volatile List<UserToken> userTokenList;

        static UserTokenRepository()
        {
            dictUserToken = new Dictionary<string, UserToken>();
            userTokenList = new List<UserToken>();
        }

        /// <summary>
        /// 添加Token
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public object Add(UserToken item)
        {
            if (string.IsNullOrEmpty(item.Token))
                throw new BusinessErrorException(BusinessError.InvalidToken);

            lock (dictUserToken)
            {
                if (!dictUserToken.ContainsKey(item.Token))
                {
                    dictUserToken.Add(item.Token, item);
                    userTokenList.Add(item);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 根据用户ID获取用户Token
        /// </summary>
        /// <param name="idKey">用户ID</param>
        /// <returns>返回用户实体</returns>
        public UserToken FindByID(string idKey)
        {
            return userTokenList.Find(d=>d.UserId==idKey);
        }

        /// <summary>
        /// 根据用户登录进去的Token查找用户实体
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserToken FindByToken(string token)
        {
            if (dictUserToken.ContainsKey(token))
            {
                var tokenEntity = dictUserToken[token];
                return tokenEntity;
                //if (tokenEntity.ExpireTime > DateTime.Now)
                //{
                //    return tokenEntity;
                //}
                ////过期
                //else
                //{
                //    lock (dictUserToken)
                //    {
                //        dictUserToken.Remove(token);
                //        userTokenList.Remove(tokenEntity);
                //    }
                //}
            }
            return null;
        }

        /// <summary>
        /// 返回所有用户实体
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserToken> GetAll()
        {
            return userTokenList;
        }

        /// <summary>
        /// 删除用户Token
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(UserToken item)
        {
            lock (dictUserToken)
            {
                if (dictUserToken.ContainsKey(item.Token))
                {
                    dictUserToken.Remove(item.Token);
                    userTokenList.Remove(item);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据用户ID查找用户Token
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>成功或失败</returns>
        public bool RemoveById(string id)
        {
            var userToken = FindByID(id);
            if(userToken != null)
            {
               return  Remove(userToken);
            }
            return false;
        }

        /// <summary>
        /// 根据用户Token删除用户
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool RemoveByToken(string token)
        {
            lock (dictUserToken)
            {
                if (dictUserToken.ContainsKey(token))
                {
                    userTokenList.Remove(dictUserToken[token]);
                    dictUserToken.Remove(token);
                    return true;                
                }
                return false;
            }
        }

        /// <summary>
        /// 更新用户Token信息
        /// </summary>
        /// <param name="item">用户Token</param>
        /// <param name="token">token信息</param>
        /// <returns></returns>
        public bool Update(UserToken item, object token)
        {
            throw new NotImplementedException();
        }

        
        /// <summary>
        /// 更新token对应的过期时间
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public void UpdateTokenExpireTime(string token)
        {
            var userToken= FindByToken(token);
            userToken.ExpireTime = DateTime.Now.AddHours(1);
        }
    }
}
