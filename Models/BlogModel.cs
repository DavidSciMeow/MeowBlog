using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MeowBlog.Models
{
    /// <summary>
    /// 博客类
    /// </summary>
    public class BlogModel
    {
        /// <summary>
        /// 唯一识别符
        /// </summary>
        public string id;
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 描述
        /// </summary>
        public string description;
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime publish;
        /// <summary>
        /// 是否对外可见
        /// </summary>
        public bool isVisiable = true;
        /// <summary>
        /// 内部字
        /// </summary>
        public string Text;
    }

    /// <summary>
    /// 博客管理类
    /// </summary>
    public static class BlogManagement
    {
        /// <summary>
        /// 博客列
        /// </summary>
        public static Dictionary<string,(string name,bool _isVisiable,DateTime publish)> Blogs { get; set; } = new();
        /// <summary>
        /// 虚拟存储位置
        /// </summary>
        public static string VPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ProgramProperties.BlogPath ?? "blogs/");
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="bm"></param>
        public static void Save(this BlogModel bm)
        {
            XmlSerializer s = new(typeof(BlogModel));
            using FileStream fs = new(Path.Combine(VPath, $"{bm.id}.blog"), FileMode.Create);
            s.Serialize(fs, bm);
            Blogs.Add(bm.id, (bm.name,bm.isVisiable,bm.publish));
            Task.Run(async () => {
                await Backup();
                $"{bm.id} Add backup Complete".ToLog(0);
            });
            fs.SafeFileHandle.Close();
        }
        /// <summary>
        /// 删除模式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int Delete(this string id)
        {
            var p = Path.Combine(VPath, $"{id}.blog");
            if (File.Exists(p))
            {
                try
                {
                    File.Delete(p);
                    Blogs.Remove(id);
                    Task.Run(async () => {
                        await Backup();
                        $"{id} Remove Item Backup Complete".ToLog(0);
                    });
                }
                catch (Exception ex)
                {
                    $"{ex.Message} occur -> in BlogDelete on Blogid:{id}".ToLog(3);
                    return 1;
                }
                return 0;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 删除模式
        /// </summary>
        /// <returns>0:成功,1:出现问题,-1:文件不存在</returns>
        public static int Delete(this BlogModel bm) => Delete(bm.id);
        /// <summary>
        /// 转换模式
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static BlogModel? DeserilizeBlogToModel(string name)
        {
            XmlSerializer s = new(typeof(BlogModel));
            try
            {
                using FileStream fs = new(Path.Combine(VPath, $"{name}.blog"), FileMode.Open);
                var k = s.Deserialize(fs);
                if (k is BlogModel)
                {
                    $"{name} Blog is Converted Complete".ToLog(0);
                    return k as BlogModel;
                }
                else
                {
                    $"{name} Blog is not Good Type, maybe is Corruppted".ToLog(1);
                }
            }
            catch
            {
                $"{name} Blog is not exist".ToLog(1);
            }
            return null;
        }
        /// <summary>
        /// 初始化模式
        /// </summary>
        public static void Init()
        {
            try
            {
                var f = File.ReadAllText(Path.Combine(VPath, "zlist.bloglist"));
                var d = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, (string name, bool _isVisiable, DateTime publish)>>(f);
                if (d is not null)
                {
                    Blogs = d;
                    $"Blog list Restore".ToLog(0);
                }
                else
                {
                    $"Blog list Empty".ToLog(0);
                }
            }
            catch
            {
                File.Create(Path.Combine(VPath, "zlist.bloglist"));
                $"Blog list Create".ToLog(0);
            }
        }
        /// <summary>
        /// 任务备份模式
        /// </summary>
        /// <returns></returns>
        public static Task Backup() => File.WriteAllTextAsync(Path.Combine(VPath, "zlist.bloglist"), Newtonsoft.Json.JsonConvert.SerializeObject(Blogs));
        /// <summary>
        /// 任务读取模式
        /// </summary>
        /// <returns></returns>
        public static Task SyncUp()
        {
            var d = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, (string, bool, DateTime)>>(File.ReadAllText(Path.Combine(VPath, "zlist.bloglist")));
            if(d is not null)
            {
                Blogs = d;
                $"SyncComplete".ToLog(1);
                return Task.CompletedTask;
            }
            else
            {
                $"SyncErr".ToLog(3);
                return Task.CompletedTask;
            }
        }
        /// <summary>
        /// 强制同步文件夹内的数据
        /// </summary>
        /// <returns></returns>
        public static Task ForceSyncUp()
        {
            var fls = new DirectoryInfo(Path.Combine(VPath)).GetFiles();
            foreach (var f in fls)
            {
                if (f.Name.EndsWith(".blog"))//是博客
                {
                    var rf = File.ReadAllText(f.FullName);//读
                    var fx = Newtonsoft.Json.JsonConvert.DeserializeObject<BlogModel>(rf);//转
                    if (fx is not null)//非空
                    {
                        var fxt = Blogs.TryAdd(fx.id, (fx.name, fx.isVisiable, fx.publish));//加
                        if (fxt)//是否存在
                        {
                            $"{fx.id} Blog have already sync into dict.".ToLog(1);//不存在,成功添加
                        }
                        else
                        {
                            $"{fx.id} Blog have already exist into dict.".ToLog(2);//存在,不得重复
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
        /// <summary>
        /// 根据Context判断是否为博主
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsRoot(this ISession s) => 0 == s.GetInt32("isRoot");
    }
}
