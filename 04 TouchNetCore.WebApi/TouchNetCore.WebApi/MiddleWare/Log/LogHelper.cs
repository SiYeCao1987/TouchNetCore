using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TouchNetCore.WebApi.MiddleWare.Log
{
    public class LogHelper
    {
        private static readonly ILog _logerror = LogManager.GetLogger(InitRepository.LoggerRepository.Name, "logerror");
        private static readonly ILog _loginfo = LogManager.GetLogger(InitRepository.LoggerRepository.Name, "loginfo");

        #region 全局异常错误记录持久化
        /// <summary>
        /// 全局异常错误记录持久化
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        public static void ErrorLog(string throwMsg, Exception ex)
        {
            string errorMsg = string.Format("【OutPut】：{0} <br>【ExceptionType】：{1} <br>【ExceptionInfo】：{2} <br>【StackTrace】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            errorMsg = errorMsg.Replace("Position", "<strong style=\"color:red\">Position</strong>");
            _logerror.Error(errorMsg);
        }
        #endregion

        #region 自定义操作记录
        /// <summary>
        /// 自定义操作记录，与仓储中的增删改的日志是记录同一张表
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        public static void WriteLog(string throwMsg, Exception ex)
        {
            string errorMsg = string.Format("【OutPut】：{0} <br>【ExceptionType】：{1} <br>【ExceptionInfo】：{2} <br>【StackTrace】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
            errorMsg = errorMsg.Replace("\r\n", "<br>");
            errorMsg = errorMsg.Replace("Position", "<strong style=\"color:red\">Position</strong>");
            _loginfo.Info(errorMsg);
        }
        #endregion
    }
}
