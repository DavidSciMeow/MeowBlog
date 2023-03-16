using Microsoft.AspNetCore.ResponseCompression;
using static MeowBlog.MainUtil;

MeowBlog.ProgramProperties.CheckPropertiesMatch(); //��ʼ����������
MeowBlog.Models.BlogManagement.Init(); //��ʼ�������ڲ��б�

var builder = WebApplication.CreateBuilder();
builder.Services.AddControllersWithViews();

builder.Services.Configure<IISServerOptions>(config => config.AllowSynchronousIO = true); //����ͬ������IO��
builder.Services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(config => config.AllowSynchronousIO = true); //����ͬ������IO��
builder.Services.Configure<Microsoft.AspNetCore.Server.HttpSys.HttpSysOptions>(config => config.AllowSynchronousIO = true); //����ͬ������IO��

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
    //ʹ����Ӧѹ��
    $"## RespCompression Serivce ON ##".ToLog(2);
}
/* 
 * ��ӷֲ�ʽ�ڴ滺��,
 * �����˽����,
 * �����:https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-6.0 
 */
if (MeowBlog.ProgramProperties.Service_Use_DMC)
{
    builder.Services.AddDistributedMemoryCache(); 
    $"## Cache Serivce ON ##".ToLog(2);
}
builder.Services.AddSession(options => //���Session
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new MeowBlog.ColoredConsoleLoggerProvider(new MeowBlog.ColoredConsoleLoggerConfiguration())); //�Զ���ASP�ڲ���־��¼��ʽ


var app = builder.Build();

/*
 * ����������ֶ��޸�Դ��,
 * ��ע���·����м������˳��,
 * �����ǰ���һ��˳�������,
 * ��ϸ˳����鿴:https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-6.0#order 
 */

app.UseExceptionHandler("/Home/Error"); //Ĭ�ϵĴ�����ҳ��
if (MeowBlog.ProgramProperties.Service_Use_HSTS)
{
    app.UseHsts(); //ʹ���ϸ�HTTPS����Э��,�����ʹ���˱��м��,��ע�������ļ���url�����Ϊhttpsģʽ
    $"## HSTS ON ##".ToLog(2);
}
if (MeowBlog.ProgramProperties.Service_Use_ResponseCache)
{
    app.UseResponseCaching(); //ʹ����Ӧ����
    $"## Cache ON ##".ToLog(2);
}
if (MeowBlog.ProgramProperties.Service_Use_ResponseCompression)
{
    app.UseResponseCompression(); //ʹ����Ӧѹ��
    $"## RespCompression ON ##".ToLog(2);
}
app.UseStaticFiles(); //����wwwroot�ļ���
app.UseRouting(); //����·��֧��
app.UseAuthorization(); //ʹ����֤
app.UseSession(); //ʹ�ûỰ��ʶ��
app.MapControllerRoute("Default", "{controller=Home}/{action=Index}");
app.MapControllerRoute("BlogDefault", "{controller}/{action}/{id}");

if (!string.IsNullOrEmpty(MeowBlog.ProgramProperties.BaseUrl))
{
    await app.RunAsync(MeowBlog.ProgramProperties.BaseUrl); //������
}
else
{
    $"Configuation Files Errs".ToLog(3);
}
$"\n- Program Shuts Down Completely".ToLog();
