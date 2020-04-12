using Framework.Core.IRepository.IUnitOfWork;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISqlSugarClient _sqlSugarClient;

        public UnitOfWork(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
            _sqlSugarClient.Aop.OnLogExecuting = (sql, pars) => //SQL执行中事件
            {
                Parallel.For(0, 1, e =>
                {
                    //MiniProfiler.Current.CustomTiming("SQL：", GetParas(pars) + "【SQL语句】：" + sql);
                    //LogLock.OutSql2Log("SqlLog", new string[] { GetParas(pars), "【SQL语句】：" + sql });

                });
            };
        }

        /// <summary>
        /// 获取DB，保证唯一性
        /// </summary>
        /// <returns></returns>
        public SqlSugarClient GetDbClient()
        {
            // 必须要as，后边会用到切换数据库操作
            return _sqlSugarClient as SqlSugarClient;
        }

        public void BeginTran()
        {
            GetDbClient().BeginTran();
        }

        public void CommitTran()
        {
            try
            {
                GetDbClient().CommitTran(); 
            }
            catch (Exception ex)
            {
                GetDbClient().RollbackTran();
                throw ex;
            }
        }

        public void RollbackTran()
        {
            GetDbClient().RollbackTran();
        }

    }

}
