![�����û�п�����ͼ,��Ӧ�ó��Է�ǽ�鿴����](https://github.com/DavidSciMeow/MeowBlog/blob/master/_gitstaticuse/Title.png)  
### һ�� `����` `������` �� Asp `(.net6)` ���͹����;  

## 0.���¸���

//��ӭ��λǰ�˹���ʦ�����ҳ��  
2022 03 18 �� ./Properties/blog.json �ƶ��� ./blog.json (�����Ŀ¼),�������.  
2022 03 20 �� dll��MarkdownParser���ĳ�js�汾.����������markdown�༭��

## 1.���

```csharp
List<String> �ŵ� = new(){
	"ǰ��ʹ����Markdown���ı�������,���������ı�д����."
	"���м�,�����������κ��������.",
	"���������Markdown�﷨�Ľ���.",
	"֧���ں�����д.",
	"֧�ֱ��������Լ��İ�ȫ����(��֤������д)",
	"֧���Լ������Լ���ǰ��ģʽ(��ʽ��д)"
}
List<String> �ƻ� = new(){
	"ǰ����ʽ","����sql��ʽ��ɾ","���Ӷ�����߼�"
}
```

## 2.�߼���Ա��������Ϣ
`sessionkeys`: [ isRoot:int {0} //���ڶԲ��Ƿ��ǲ���, loginTime //��½ʱ�� ]  
`session��Ч��`: 1Сʱ //�����������Programs.cs���ĺ����±���  
`API��ֵ`: string:: / [result:int,errs:strings] //�����API�ķ���ֵΪ�ַ���,����ʹ��ԭ��js����  
`���������ļ�`: wwwroot/blogs/zlist.bloglist  
> `�����ɸ��������Ѿ��������ļ����г���ע��`  
> `�����ļ�����λ�� /Properties/blog.json (·����Ҫ��׼)`  
```javascript
//���Ͳ���ʹ�õ�Json��ʽ
{
	"name":string,
	"desc":string,
	"visable":string, // yes, no
	"text":string,
}
```

## 3.����ʹ�÷���(�ȴ�����)
> 2022��3��28�� :   
>1. Folk���� ʹ��VS2022��, 
>1. ��������ʽ�����������������
>1. ����, ȷ�Ϲ�������
>1. ʹ��Publishģʽ�ַ�.
>1. ʹ��Nginx/Apache���д���������������д�ĵ�ַ,��ƥ������.
>1. ������Ŀ¼�����в��ͳ���. `dotnet ./MeowBlog`
>1. ���������������ʲ���.
>1. ���.
