![如果您没有看到本图,您应该尝试翻墙查看本库](https://github.com/DavidSciMeow/MeowBlog/blob/master/_gitstaticuse/Title.png)  
### 一个 `极其` `轻量化` 的 Asp `(.net6)` 博客管理端;  

## 0.最新更新

//欢迎各位前端工程师来设计页面  
2022 03 18 将 ./Properties/blog.json 移动至 ./blog.json (程序根目录),方便查找.  
2022 03 20 将 dll的MarkdownParser更改成js版本.并且适配了markdown编辑器

## 1.简介

```csharp
List<String> 优点 = new(){
	"前端使用了Markdown富文本解析器,增进了您的编写体验."
	"运行简单,轻量化不需任何其他组件.",
	"适配操作于Markdown语法的解析.",
	"支持内函数重写.",
	"支持编译属于自己的安全博客(验证机制重写)",
	"支持自己更改自己的前端模式(样式重写)"
}
List<String> 计划 = new(){
	"前端样式","适配sql方式增删","增加多核心逻辑"
}
```

## 2.高级人员调试用信息
`sessionkeys`: [ isRoot:int {0} //用于对查是否是博主, loginTime //登陆时间 ]  
`session有效期`: 1小时 //如需更改请在Programs.cs更改后重新编译  
`API回值`: string:: / [result:int,errs:strings] //大多数API的返回值为字符串,而后使用原生js解析  
`快速索引文件`: wwwroot/blogs/zlist.bloglist  
> `其他可更改配置已经在设置文件中列出并注释`  
> `设置文件复制位置 /Properties/blog.json (路径需要对准)`  
```javascript
//发送博客使用的Json格式
{
	"name":string,
	"desc":string,
	"visable":string, // yes, no
	"text":string,
}
```
