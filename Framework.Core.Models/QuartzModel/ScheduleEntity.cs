using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models
{
    /// <summary>
    /// 任务调度实体
    /// </summary>
    public class ScheduleEntity : RootEntity
    {
        /// <summary>
        /// 任务分组
        /// </summary>
        [SugarColumn(ColumnDescription = "任务分组")]
        public string JobGroup { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        [SugarColumn(ColumnDescription = "任务名称")]
        public string JobName { get; set; }
        /// <summary>
        /// 执行周期表达式
        /// </summary>
        [SugarColumn(ColumnDescription = "执行周期表达式")]
        public string Cron { get; set; }
        /// <summary>
        /// 任务运行状态
        /// </summary>
        [SugarColumn(ColumnDescription = "任务运行状态")]
        public JobRunStatus RunStatus { get; set; }
        /// <summary>
        /// 程序集名称
        /// </summary>
        [SugarColumn(ColumnDescription = "程序集名称")]
        public string AssemblyName { get; set; }
        /// <summary>
        /// Job类名
        /// </summary>
        [SugarColumn(ColumnDescription = "Job类名")]
        public string ClassName { get; set; }
        /// <summary>
        /// 执行次数
        /// </summary>
        [SugarColumn(ColumnDescription = "执行次数")]
        public int RunTimes { get; set; }
        /// <summary>
        /// 执行间隔时间, 秒为单位
        /// </summary>
        [SugarColumn(ColumnDescription = "执行间隔时间")]
        public int IntervalSecond { get; set; }
    }

    public enum JobRunStatus
    {
        Await = 0,
        run = 1,
        stop = 2
    }
}
