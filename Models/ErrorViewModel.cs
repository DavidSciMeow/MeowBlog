namespace MeowBlog.Models
{
    /// <summary>
    /// ����չʾģ��
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// ����ID
        /// </summary>
        public string? RequestId { get; set; }
        /// <summary>
        /// չʾ����ID
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}