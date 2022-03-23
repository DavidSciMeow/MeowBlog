using Microsoft.AspNetCore.ResponseCompression;
using static MeowBlog.MainUtil;

MeowBlog.ProgramProperties.CheckPropertiesMatch(); //初始化博客配置
MeowBlog.Models.BlogManagement.Init(); //初始化博客内部列表

var builder = WebApplication.CreateBuilder();
builder.Services.AddControllersWithViews();

builder.Services.Configure<IISServerOptions>(config => config.AllowSynchronousIO = true); //允许同步操作IO流
builder.Services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(config => config.AllowSynchronousIO = true); //允许同步操作IO流
builder.Services.Configure<Microsoft.AspNetCore.Server.HttpSys.HttpSysOptions>(config => config.AllowSynchronousIO = true); //允许同步操作IO流

if (MeowBlog.ProgramProperties.Service_Use_HSTS)
{
    builder.Services.AddHsts(options =>
    {
        options.Preload = true;
        options.IncludeSubDomains = true;
        options.MaxAge = TimeSpan.FromDays(365);
        //options.ExcludedHosts.Add("www.example.com");
    });
    $"## HSTS Serivce ON ##".ToLog(2);
}
if (MeowBlog.ProgramProperties.Service_Use_ResponseCompression)
{
    builder.Services.AddResponseCompression(options =>
    {
        options.Providers.Add<GzipCompressionProvider>();
    });
    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = System.IO.Compression.CompressionLevel.Fastest;
    });
    //使用响应压缩
    $"## RespCompression Serivce ON ##".ToLog(2);
}
/* 
 * 添加分布式内存缓存,
 * 如想了解更多,
 * 请参阅:https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-6.0 
 */
if (MeowBlog.ProgramProperties.Service_Use_DMC)
{
    builder.Services.AddDistributedMemoryCache(); 
    $"## Cache Serivce ON ##".ToLog(2);
}
builder.Services.AddSession(options => //添加Session
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new MeowBlog.ColoredConsoleLoggerProvider(new MeowBlog.ColoredConsoleLoggerConfiguration())); //自定义ASP内部日志记录方式


var app = builder.Build();

/*
 * 如果您正在手动修改源码,
 * 请注意下方的中间件排序顺序,
 * 他们是按照一定顺序排序的,
 * 详细顺序请查看:https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0#order 
 */

app.UseExceptionHandler("/Home/Error"); //默认的错误处理页面
if (MeowBlog.ProgramProperties.Service_Use_HSTS)
{
    app.UseHsts(); //使用严格HTTPS传输协议,如果您使用了本中间件,请注意配置文件的url域必须为https模式
    $"## HSTS ON ##".ToLog(2);
}
if (MeowBlog.ProgramProperties.Service_Use_ResponseCache)
{
    app.UseResponseCaching(); //使用响应缓存
    $"## Cache ON ##".ToLog(2);
}
if (MeowBlog.ProgramProperties.Service_Use_ResponseCompression)
{
    app.UseResponseCompression(); //使用响应压缩
    $"## RespCompression ON ##".ToLog(2);
}
app.UseStaticFiles(); //访问wwwroot文件夹
app.UseRouting(); //启用路由支持
app.UseAuthorization(); //使用认证
app.UseSession(); //使用会话标识符
app.MapControllerRoute("Default", "{controller=Home}/{action=Index}");
app.MapControllerRoute("BlogDefault", "{controller}/{action}/{id}");

if (!string.IsNullOrEmpty(MeowBlog.ProgramProperties.BaseUrl))
{
    await app.RunAsync(MeowBlog.ProgramProperties.BaseUrl); //读配置
}
else
{
    $"Configuation Files Errs".ToLog(3);
}
$"\n- Program Shuts Down Completely".ToLog();
