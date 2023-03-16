namespace MeowBlog.Models
{
    /// <summary>
    /// 错误展示模块
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// 请求ID
        /// </summary>
        public string? RequestId { get; set; }
        /// <summary>
        /// 展示请求ID
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}