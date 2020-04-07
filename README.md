# Framework.Core
.Net Core 3.1前后端分离开发框架（切面日志、缓存、事务、依赖注入、按钮级别权限管理）-   NET Core后端
## 项目截图
<img src='https://raw.githubusercontent.com/Post781937858/Framework.Core/master/Framework.Core/images/uploader/Icon/main.PNG'>
<img src='https://raw.githubusercontent.com/Post781937858/Framework.Core/master/Framework.Core/images/uploader/Icon/main2.PNG'>
<img src='https://raw.githubusercontent.com/Post781937858/Framework.Core/master/Framework.Core/images/uploader/Icon/mian1.PNG'>
<img src='https://raw.githubusercontent.com/Post781937858/Framework.Core/master/Framework.Core/images/uploader/Icon/mian3.PNG'>
<img src='https://raw.githubusercontent.com/Post781937858/Framework.Core/master/Framework.Core/images/uploader/Icon/mian4.PNG'>
<img src='https://raw.githubusercontent.com/Post781937858/Framework.Core/master/Framework.Core/images/uploader/Icon/mian5.PNG'>
# Vue+.NetCore前后端分离，支持对前端、后台基础业务代码扩展的快速发开框架
## 框架可直上手开发这些功能
 - Vol.WebApi类库可独立用于restful api服务单独部署,用于其他系统单独提供接口,直接上手编写业务代码即可。
 -  Vue+Vol.WebApi 可用于现有框架前后端分离进行开发
 - Vol.Web类库可用于传统MVC+Razor方式进行项目开发
 -  Vol.Builder类库可作为一个独立的代码生成器,可生成cshtml页面、Vue页面、Model文件、Service与Repository.cs业务处理代码类
 -  可作为一个独立站点来发布Editor编辑器生成的静态html网页.
 - 可直接用于H5移动App开发H5开发看这里

## 框架特点
 - 支持前端、后台基础业务代码动态扩展，可在现有框架增、删、改、查、导入、导出、审核基础业务上扩展复杂的业务代码
 - 基本业务全部由框架完成，上手即可对基础业务以外的代码进行扩展
 - 上手简单，需要.net core3.1、VsCode mysql/sqlservcer 2012、redis(可选) 及以上版本的开发环境
 - 学习成本低，封装了常用可扩展组件及Demo(前端基于Iview/Element-UI组件进行了二次封装、后台提供了大量的扩展方法)
 - 开发效率高,内定制开发的代码生成器,生成前端(Vue、后台代码),代码生成器已完成90%以上的重复工作，只需要在提供的扩展类型中实现其他业务
 - 前端vue页面表单下拉/多选框完成自动绑定数据源，不需要写任何代码,并支持扩展自定开发绑定。
 - 后台已完成权限、菜单、JWT等内部功能
 
如果你没有做过webpack+vue工程化开发项目，可能会刚开始相当不适应，或者安装环境总是出问题，但只要你熟悉开发流程后，你会发现采用Vue开发比Jquery爽太多了。上手项目需重点了解基础Vue语法，特别是了解组件、路由及import的使用


## 开发及依赖环境
VS2017 、.NetCore3.1 、EFCore3.1、JWT、Dapper、Autofac、SqlServer/MySql、Redis(可选，没有redis的在appsetting.json中不用配置，默认使用内置IMemory)、<br>VsCode、Vue2.0（webpack、node.js,如果没有此环境自行搜索:vue webpack npm)、Vuex、axios、promise、IView、Element-ui

## 项目运行
如果你没有前端环境，请先安装node.js, 前端开发使用VsCode  

 - 1、使用cmd命令切换至前端Vue项目.../VOL.Vue路径下,执行npm install命令(只有从来没执行过此命令的才执行npm install)
 - 2、运行后端项目：在后端项目路径.../VOL.WebApi/运行dev_run.bat端口设置的是9991,运行前先看appsettings.josn配置属性说明
 - 3、运行前端项目：在前端Vue项目路径.../VOL.Vue/运行run.bat（每次启动会进行编译，这个时间可能会有点长）
 - 4、输入http://localhost:8080访问  
## 功能介绍
![功能介绍](https://github.com/cq-panda/Vue.NetCore/blob/master/imgs/func.png)  
 
![Home](https://github.com/cq-panda/Vue.NetCore/blob/master/imgs/home.png)  

## 1、只读基础表单
整个只读的基础表单的所有前后端代码，全部由代码生成器生成，代码生成器中几乎不需要配置，并支持并后端业务代码扩展，直接生成代码后，配置菜单权限即可
![Home](https://github.com/cq-panda/Vue.NetCore/blob/master/imgs/table1.png)  

 
