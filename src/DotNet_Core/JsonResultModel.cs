using System;
using System.Collections.Generic;
using System.Text;

namespace System.Web.Mvc
{
    public class JsonResultModel
    {
        /// <summary>
        /// JsonResult类的data参数封装
        ///  20171201 xzx add
        /// </summary>
        public JsonResultModel()
        {
            HandleResult = (Int32)HandleResultEnum.Succeed;
            DetailError = new List<string>();
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        public Int32 HandleResult { get; set; }
        /// <summary>
        /// 详细错误信息
        /// </summary>
        public List<string> DetailError { get; set; }

        /// <summary>
        /// 数据库记录的异常信息ID(未记录为null)
        /// </summary>
        public Guid? LogID { get; set; }
        /// <summary>
        /// 异常请求对应的URL
        /// </summary>
        public string ExceptionUrl { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// 异常跟踪
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// 返回数据(JSON)
        /// </summary>
        public Object Data { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 处理数
        /// </summary>
        public int HandleCount { get; set; }
    }

    /// <summary>
    /// 执行结果枚举
    /// </summary>
    public enum HandleResultEnum
    {
        /// <summary>
        /// 执行失败 = 0
        /// </summary>
        Failure = 0,
        /// <summary>
        /// 执行成功 = 1
        /// </summary>
        Succeed = 1,
        /// <summary>
        /// 部分执行失败 = 2
        /// </summary>
        Defects = 2
    }
}
