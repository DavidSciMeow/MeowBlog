
namespace MeowBlog
{
    /// <summary>
    /// 程序属性
    /// </summary>
    public static class ProgramProperties
    {
        /*内部存储*/
        static IConfigurationRoot? blog;
        static bool _isConfiged = false;

        /*程序标准配置*/
        public static bool Service_Use_HSTS { get; set; } = false;
        public static bool Service_Use_DMC { get; set; } = false;
        public static bool Service_Use_ResponseCache { get; set; } = false;
        public static bool Service_Use_ResponseCompression { get; set; } = false;

        /*基础配置*/
        public static string? BaseUrl { get; set; } //底,监听段
        public static string? Password { get; set; } //管理者密码
        public static int Loglevel { get; set; } = 0; //日志记录程度

        /*公共配置*/
        public static string? BlogPath { get; set; } //存储位置
        public static int BlogType { get; set; } //存储类型
        public static string? BootstrapVersion { get; set; } //Bootstrap版本
        public static string? JQueryVersion { get; set; } //Jquery版本

        /*页面配置*/
        public static string? InnerIndexLayoutPage { get; set; } //主页面内部配置
        public static string? IndexName { get; set; } //主页名称
        public static string? BlogName { get; set; } //主页名称
        public static string? IndexNavPage { get; set; } //导航栏方案页面

        /// <summary>
        /// 获得自定义配置
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string? GetCustomizedConfig(string Key) => blog?[Key]?.ToString();
        /// <summary>
        /// 初始化默认属性
        /// </summary>
        /// <returns>是否存在文件的初始化成功或者已经初始化成功</returns>
        public static bool CheckPropertiesMatch() //初始化用
		{
            if (!_isConfiged) //如果已经配置过则忽略配置读取内存配置
            {
                try //尝试加载文件, 若不存在则执行初始化
                {
                    blog = new ConfigurationBuilder().AddJsonFile("./blog.json").Build(); //设置文件目录
                    BaseUrl = blog["BaseListen"];
                    BlogPath = blog["BlogPath"];
                    var pblogs = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", BlogPath ?? "blogs/");
                    if (!Directory.Exists(pblogs))
                    {
                        $"blogs Directory Not Exist, Creating.".ToLog(2);
                        Directory.CreateDirectory(pblogs);
                    }
                    BootstrapVersion = blog["BootstrapVersion"];
                    JQueryVersion = blog["JqueryVersion"];
                    InnerIndexLayoutPage = blog["InnerIndexLayoutPage"];
                    IndexName = blog["IndexName"];
                    IndexNavPage = blog["IndexNavPage"];
                    BlogName = blog["BlogName"];
                    Service_Use_HSTS = "yes".Equals(blog["Use_HSTS"]);
                    Service_Use_DMC = "yes".Equals(blog["Use_DMC"]);
                    Service_Use_ResponseCache = "yes".Equals(blog["Use_ResponseCache"]);
                    Service_Use_ResponseCompression = "yes".Equals(blog["Use_ResponseCompression"]);
                    Password = blog["Password"];
                    Loglevel = int.Parse(blog["LogLevel"]);
                    BlogType = int.Parse(blog["StorageType"]);
                }
                catch(Exception ex)
                {
                    $"ERRS: [Undecided Properties Files]\n{ex}".ToLog(3); //未加载配置文件或配置文件出错
                    return false;
                }
            }
            _isConfiged = true;
            return true;
        }
    }
}
