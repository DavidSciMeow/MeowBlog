using MeowBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;

namespace MeowBlog.Controllers
{
    /// <summary>
    /// 网页相关控制器 C/
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {

            try
            {
                ViewData["innerHtml"] = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ProgramProperties.InnerIndexLayoutPage ?? "Basic/IndexLayout.html"));
            }
            catch
            {
                ViewData["innerHtml"] = "<div class=\"text-center\"><h1>Welcome Using Meow's Blog</h1><p>Please using IndexLayout.html to change this page</p><h2>欢迎使用喵的微型博客</h2><p>请设置Basic内的IndexLayout.html页更改本内容</p></div>";
            }
            return View();
        }
        /// <summary>
        /// 登入展示页
        /// </summary>
        /// <returns></returns>
        public IActionResult Login() => View();
        /// <summary>
        /// 登入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ApiLogin()
        {
            try
            {
                var jo = JObject.Parse(new StreamReader(Request.HttpContext.Request.Body, Encoding.Default).ReadToEnd());
                if (ProgramProperties.Password?.Equals(jo["pass"]?.ToString()) ?? false)
                {
                    Request.HttpContext.Session.SetInt32("isRoot", 0);
                    Request.HttpContext.Session.SetString("loginTime", $"{DateTime.Now:d}");
                    return Content("0:null");
                }
                else
                {
                    return Content("1:passerr");
                }

            }
            catch (Exception ex)
            {
                $"On Method *Home-Login* Throw Err follows:{ex.Message}".ToLog(3);
                return Content($"-1:{ex.Message}");
            }
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public IActionResult ApiLogout()
        {
            Request.HttpContext.Session.Clear();
            return Redirect("/Home/Index");
        }
        /// <summary>
        /// 错误展示
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}