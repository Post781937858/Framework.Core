using Framework.Core.Common;
using Framework.Core.Models;
using Framework.Core.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Reflection;

namespace Framework.Core.CodeTemplate
{
    public class CodeContext : ICodeContext
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly TemplateConfig templateConfig;
        private readonly IWebHostEnvironment env;

        public CodeContext(ISqlSugarClient sqlSugarClient, TemplateConfig templateConfig, IWebHostEnvironment env)
        {
            this._sqlSugarClient = sqlSugarClient;
            this.templateConfig = templateConfig;
            this.env = env;
        }

        /// <summary>
        /// 获取所有表信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<TableInfo>> MysqlGetTablesAsync()
        {
            return (await _sqlSugarClient.SqlQueryable<MysqlTableInfo>($"SELECT * FROM information_schema.TABLES  WHERE table_schema='{DBConfig.DbName}'")
                .ToListAsync())
                .Select(p => new TableInfo()
                {
                    Name = p.TABLE_NAME,
                    Brief = string.IsNullOrEmpty(p.TABLE_MOMMENT) ? "无" : p.TABLE_MOMMENT,
                    RowsCount = p.TABLE_ROWS,
                    CreateTime = p.CREATE_TIME,
                    UpdateTime = p.UPDATE_TIME,
                    encode = p.TABLE_COLLATION
                }).ToList();
        }

        /// <summary>
        /// 获取字段信息
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public async Task<List<TableFieldInfo>> MysqlGetTableFieldAsync(string TableName)
        {
            return (await _sqlSugarClient.SqlQueryable<MysqlTableFieldInfo>($"select COLUMN_NAME,COLUMN_COMMENT,DATA_TYPE,IS_NULLABLE from information_schema.COLUMNS where table_schema='{DBConfig.DbName}' and  TABLE_NAME='{TableName}'")
                 .ToListAsync()).Select(p => new TableFieldInfo()
                 {
                     ColDbType = p.DATA_TYPE,
                     IsNullAble = p.IS_NULLABLE.ToUpper() == "YES",
                     Brief = string.IsNullOrEmpty(p.COLUMN_COMMENT) ? "无" : p.COLUMN_COMMENT,
                     ColName = p.COLUMN_NAME,
                     TableName = TableName,
                     ColType = ConvertToDBType.MysqlChangeDBTypeToCSharpType(p.DATA_TYPE)
                 }).ToList();
        }

        /// <summary>
        /// 反射获取所有模型
        /// </summary>
        /// <returns></returns>
        public List<modelInfo> GetModelInfos(string modelName = "")
        {
            List<modelInfo> modelInfos = new List<modelInfo>();
            var Types = typeof(RootEntity).Assembly.GetTypes().Where(p => p.BaseType == typeof(RootEntity));
            foreach (var item in Types)
            {
                var attribute = item.GetCustomAttributes(true).FirstOrDefault(s => s.GetType() == typeof(ModelDescriptionAttribute));
                var description = attribute != null ? (attribute as ModelDescriptionAttribute).Description : "无";
                modelInfos.Add(new modelInfo() { modelName = item.Name, Description = description });
            }
            if (!string.IsNullOrEmpty(modelName))
            {
                return modelInfos.Where(p => p.modelName.Contains(modelName)).ToList();
            }
            return modelInfos;
        }

        /// <summary>
        /// 反射获取模型属性
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public List<modelProperty> GetProperty(string modelName)
        {
            List<modelProperty> modelPropertys = new List<modelProperty>();
            if (!string.IsNullOrEmpty(modelName))
            {
                var Type = typeof(RootEntity).Assembly.GetTypes().FirstOrDefault(p => p.Name.ToLower() == modelName.ToLower());
                if (Type != null)
                {
                    var Properties = Type.GetProperties();
                    foreach (var item in Properties)
                    {
                        var attribute = item.GetCustomAttributes(true).FirstOrDefault(s => s.GetType() == typeof(SugarColumn));
                        var description = attribute != null ? (attribute as SugarColumn).ColumnDescription : "无";
                        var columnType = item.PropertyType.Name;
                        if (item.PropertyType.BaseType == typeof(System.Enum))
                        {
                            columnType = typeof(System.Enum).Name;
                        }
                        modelPropertys.Add(new modelProperty() { ColumnName = item.Name, ColumnType = columnType, ColumnDescription = description, propertyInfo = item });
                    }
                }
            }
            return modelPropertys;
        }


        /// <summary>
        /// 创建前端页面
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="Brief"></param>
        /// <param name="codeView"></param>
        /// <returns></returns>
        public string CreateVueCode(string modelName, string Brief, CodeView codeView)
        {
            List<modelProperty> Propertys = codeView.Propertys;
            var modelPropertys = GetProperty(modelName);
            TemplateEngine templateEngine = new TemplateEngine();
            templateEngine.SetFile("Tem", templateConfig.VueTemplateFile);
            templateEngine.SetBlock("Tem", "FieldList1", "list1", "<!--\\s+BEGIN FieldList1\\s+-->([\\s\\S.]*)<!--\\s+END FieldList1\\s+-->");
            templateEngine.m_noMarkers = "comment";
            templateEngine.SetVal("t_name", modelName, false);
            if (!codeView.tableToolOn)
            {
                templateEngine.SetVal("m_Istool", "", false);
            }
            else
            {
                templateEngine.SetVal("m_Istool", "v-if='false'", false);
            }
            if (Propertys.Any())
            {
                StringBuilder queryparameter = new StringBuilder();
                foreach (var item in Propertys)
                {
                    if (item.ColumnType == typeof(string).Name)
                    {
                        queryparameter.Append($"{item.ColumnName}: '',\r\n");
                    }
                    else
                    {
                        queryparameter.Append($"{item.ColumnName}: null,\r\n");
                    }
                    item.propertyInfo = modelPropertys.FirstOrDefault(p => p.ColumnName.ToLower() == item.ColumnName.ToLower()).propertyInfo;
                }
                templateEngine.SetVal("b_queryparameter", queryparameter.ToString(), false);
                templateEngine.SetVal("f_queryformitem", GroupFormItem(Propertys, "QueryForm", false), false);
            }
            else
            {
                templateEngine.SetVal("b_ queryparameter", "", false);
            }
            var DlogForm = GroupFormItem(modelPropertys.Where(p => p.ColumnName != "RootEntity").ToList(), "ruleForm", true);
            templateEngine.SetVal("dlog_formItem", DlogForm, false);
            StringBuilder verification = new StringBuilder();
            foreach (var item in modelPropertys)
            {
                var ColName = item.ColumnName.Substring(0, 1).ToLower() + item.ColumnName.Remove(0, 1);
                verification.Append("{item}: [{ required: true, message: '不能为空', trigger: 'blur' }],".Replace("{item}", ColName));
                if (item.ColumnName.ToLower() == "id") continue;
                if (item.ColumnType == typeof(int).Name)
                {
                    templateEngine.SetVal("f_type", ".number", false);
                }
                else
                {
                    templateEngine.SetVal("f_type", "", false);
                }
                templateEngine.SetVal("f_name", ColName, false);
                templateEngine.SetVal("f_note", item.ColumnDescription, false);
                templateEngine.Parse($"list1", $"FieldList1", true);
            }
            string RowVerification = verification.ToString();
            templateEngine.SetVal("p_verification", RowVerification.Remove(RowVerification.Length - 1, 1), false);
            templateEngine.Parse("out", "Tem", false);
            return templateEngine.PutOutPageCode("out");
        }

        /// <summary>
        /// 拼装Form表单
        /// </summary>
        /// <param name="Propertys"></param>
        /// <param name="FormData"></param>
        /// <param name="IsQueryForm"></param>
        /// <returns></returns>
        public string GroupFormItem(List<modelProperty> Propertys, string FormData,bool IsQueryForm)
        {
            StringBuilder queryFormItem = new StringBuilder();
            foreach (var item in Propertys)
            {
                var ColName = item.ColumnName.Substring(0, 1).ToLower() + item.ColumnName.Remove(0, 1);
                if (item.ColumnType == typeof(DateTime).Name)
                {
                    queryFormItem.Append($"<el-form-item {(IsQueryForm ? "class='col-2'" : "")}  label='{item.ColumnDescription}' prop='{ColName}'><el-date-picker placeholder='{item.ColumnDescription}' v-model='{FormData}.{ColName}'></el-date-picker></el-form-item>\r\n");
                }
                else if (item.ColumnType == typeof(System.Enum).Name)
                {
                    string select = $"<el-form-item {(IsQueryForm ? "class='col-2'" : "")} label='{item.ColumnDescription}' prop='{ColName}'><el-select placeholder='{item.ColumnDescription}'  v-model='{FormData}.{ColName}'>[item]</el-select></el-form-item>\r\n";
                    var selectType = item.propertyInfo.PropertyType;
                    var optionsFields = selectType.GetFields(BindingFlags.Static | BindingFlags.Public);
                    StringBuilder options = new StringBuilder();
                    foreach (var optionItem in optionsFields)
                    {
                        var label = optionItem.Name;
                        var value = optionItem.GetRawConstantValue();
                        options.Append($"<el-option label='{label}' :value='{value}'></el-option>");
                    }
                    queryFormItem.Append(select.Replace("[item]", options.ToString()));
                }
                else if (item.ColumnType == typeof(int).Name)
                {
                    queryFormItem.Append($"<el-form-item {(IsQueryForm ? "class='col-2'" : "")} label='{item.ColumnDescription}' prop='{ColName}'><el-input placeholder='{item.ColumnDescription}' v-model.number='{FormData}.{ColName}'></el-input></el-form-item>\r\n");
                }
                else
                {
                    queryFormItem.Append($"<el-form-item {(IsQueryForm ? "class='col-2'" : "")} label='{item.ColumnDescription}' prop='{ColName}'><el-input placeholder='{item.ColumnDescription}' v-model='{FormData}.{ColName}'></el-input></el-form-item>\r\n");
                }
            }
            return queryFormItem.ToString();
        }

        /// <summary>
        /// 创建控制器类
        /// </summary>
        /// <returns></returns>
        public string CreateControllersCode(string TableName, string CommentsValue, List<modelProperty> Propertys)
        {
            TemplateEngine templateEngine = new TemplateEngine();
            templateEngine.SetFile("Tem", templateConfig.ControllersTemplateFile);
            templateEngine.m_noMarkers = "comment";
            templateEngine.SetVal("t_object", TableName, false);
            if (Propertys.Any())
            {
                StringBuilder builder = new StringBuilder();
                StringBuilder parameter = new StringBuilder();
                foreach (var item in Propertys)
                {
                    var whereItem = string.Empty;
                    if (item.ColumnType == typeof(string).Name)
                    {
                        whereItem = @"           if (!string.IsNullOrEmpty({t_item}))
                {
                    whereExpressionAll = whereExpressionAll.And(p => p.{t_item} == {t_item});
                }
                        " + "\r\n";
                    }
                    else if (item.ColumnType == typeof(int).Name || item.ColumnType == typeof(System.Enum).Name)
                    {
                        whereItem = @"            if ((int){t_item} != 0)
                    {
                        whereExpressionAll = whereExpressionAll.And(p => p.{t_item} == {t_item});
                    }" + "\r\n";
                    }
                    else
                    {
                        whereItem = "          whereExpressionAll = whereExpressionAll.And(p => p.{t_item} == {t_item});" + "\r\n";
                    }
                    if (item.ColumnType == typeof(System.Enum).Name)
                    {
                        parameter.Append($",{item.propertyInfo.PropertyType.Name}  {item.ColumnName}");
                    }
                    else
                    {
                        parameter.Append($",{item.ColumnType}  {item.ColumnName}");
                    }
                    builder.Append(whereItem.Replace("{t_item}", item.ColumnName));
                }
                var parameterWhere = parameter.ToString() + ",";
                templateEngine.SetVal("t_parameter", parameterWhere.Remove(0, 1), false);
                templateEngine.SetVal("f_parameter", builder.ToString(), false);
            }
            else
            {
                templateEngine.SetVal("t_parameter", "", false);
                templateEngine.SetVal("f_parameter", "", false);
            }
            templateEngine.Parse("out", "Tem", false);
            return templateEngine.PutOutPageCode("out");
        }

        /// <summary>
        /// 创建模型
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="Brief"></param>
        /// <returns></returns>
        public string CreateModelCode(string modelName, string Brief)
        {
            var modelPropertys = GetProperty(modelName);
            TemplateEngine templateEngine = new TemplateEngine();
            templateEngine.SetFile("Tem", templateConfig.ModelsTemplateFile);
            templateEngine.SetBlock("Tem", "FieldList1", "list1", "<!--\\s+BEGIN FieldList1\\s+-->([\\s\\S.]*)<!--\\s+END FieldList1\\s+-->");
            templateEngine.m_noMarkers = "comment";
            templateEngine.SetVal("t_name", modelName, false);
            templateEngine.SetVal("t_note", Brief, false);
            templateEngine.SetVal("t_object", modelName, false);
            templateEngine.SetVal("t_namespace", modelName, false);
            templateEngine.SetVal("t_DateTime", DateTime.Now.ToString(), false);
            foreach (var f in modelPropertys)
            {
                templateEngine.SetVal("f_name", f.ColumnName, false);
                templateEngine.SetVal("f_type", f.ColumnType, false);
                templateEngine.SetVal("f_note", f.ColumnDescription, false);
                templateEngine.Parse("list1", "FieldList1", true);
            }
            templateEngine.Parse("out", "Tem", false);
            return templateEngine.PutOutPageCode("out");
        }

        /// <summary>
        /// 创建仓储接口
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="CommentsValue"></param>
        /// <returns></returns>
        public string CreateIRepositoryCode(string TableName, string CommentsValue)
        {
            TemplateEngine templateEngine = new TemplateEngine();
            templateEngine.SetFile("Tem", templateConfig.IRepositoryTemplateFile);
            templateEngine.m_noMarkers = "comment";
            templateEngine.SetVal("t_object", TableName, false);
            templateEngine.Parse("out", "Tem", false);
            return templateEngine.PutOutPageCode("out");
        }

        /// <summary>
        /// 创建服务接口
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="CommentsValue"></param>
        /// <returns></returns>
        public string CreateIServicesCode(string TableName, string CommentsValue)
        {
            TemplateEngine templateEngine = new TemplateEngine();
            templateEngine.SetFile("Tem", templateConfig.IServicesTemplateFile);
            templateEngine.m_noMarkers = "comment";
            templateEngine.SetVal("t_object", TableName, false);
            templateEngine.Parse("out", "Tem", false);
            return templateEngine.PutOutPageCode("out");
        }

        /// <summary>
        /// 创建仓储类
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="CommentsValue"></param>
        /// <returns></returns>
        public string CreateRepositoryCode(string TableName, string CommentsValue)
        {
            TemplateEngine templateEngine = new TemplateEngine();
            templateEngine.SetFile("Tem", templateConfig.RepositoryTemplateFile);
            templateEngine.m_noMarkers = "comment";
            templateEngine.SetVal("t_object", TableName, false);
            templateEngine.Parse("out", "Tem", false);
            return templateEngine.PutOutPageCode("out");
        }

        /// <summary>
        /// 创建服务类
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="CommentsValue"></param>
        /// <returns></returns>
        public string CreateServicesCode(string TableName, string CommentsValue)
        {
            TemplateEngine templateEngine = new TemplateEngine();
            templateEngine.SetFile("Tem", templateConfig.ServicesTemplateFile);
            templateEngine.m_noMarkers = "comment";
            templateEngine.SetVal("t_object", TableName, false);
            templateEngine.Parse("out", "Tem", false);
            return templateEngine.PutOutPageCode("out");
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        public CodeView GetTemplateConfig()
        {
            return new CodeView()
            {
                ModelOutputPath = templateConfig.ModelsOutputFile,
                IRepositoryOutputPath = templateConfig.IRepositoryOutputFile,
                IServicesOutputPath = templateConfig.IServicesOutputFile,
                RepositoryOutputPath = templateConfig.RepositoryOutputFile,
                ServicesOutputPath = templateConfig.ServicesOutputFile,
                ControllersOutputPath = templateConfig.ControllersOutputFile,
                VueOutputPath = templateConfig.VueOutputFile
            };
        }

        /// <summary>
        /// 输出所有映射文本
        /// </summary>
        /// <param name="codeView"></param>
        /// <returns></returns>
        public resultCode ShowOutTemplateCode(CodeView codeView)
        {
            var model = codeView.model;
            resultCode resultCode = new resultCode();
            resultCode.VueCode += CreateVueCode(model.modelName, model.Description, codeView) + "\r\n";
            resultCode.controllerCode += CreateControllersCode(model.modelName, model.Description, codeView.Propertys) + "\r\n";
            resultCode.ModelCode +=  CreateModelCode(model.modelName, model.Description) + "\r\n";
            resultCode.IRepositoryCode += CreateIRepositoryCode(model.modelName, model.Description) + "\r\n";
            resultCode.IServicesCode += CreateIServicesCode(model.modelName, model.Description) + "\r\n";
            resultCode.ServicesCode += CreateServicesCode(model.modelName, model.Description) + "\r\n";
            resultCode.RepositoryCode += CreateRepositoryCode(model.modelName, model.Description) + "\r\n";
           
            return resultCode;
        }

        /// <summary>
        /// 创建模板映射文件
        /// </summary>
        /// <param name="codeView"></param>
        /// <returns></returns>
        public async Task OutTemplateCode(CodeView  codeView)
        {
            var model = codeView.model;
            DirectoryInfo topDir = Directory.GetParent(env.ContentRootPath);
            var BasePath = topDir.FullName;
            //var ModelPath = Path.Combine(templateConfig.ModelsOutputFile, $"{model.Name}.cs");
            //var ModelCode = await CreateModelCode(model.Name, model.Brief);
            //await File.WriteAllTextAsync(ModelPath, ModelCode);

            var VueDirectoryPath = Path.Combine(templateConfig.VueOutputFile, $"{model.modelName}\\");
            if (!Directory.Exists(VueDirectoryPath)) Directory.CreateDirectory(VueDirectoryPath);
            var VuePath = Path.Combine(templateConfig.VueOutputFile, $@"{model.modelName}\{model.modelName}.vue");
            var VueCode =  CreateVueCode(model.modelName, model.Description, codeView);
            if (!File.Exists(VuePath))
                await File.WriteAllTextAsync(VuePath, VueCode);

            var ControllersPath = Path.Combine(BasePath, templateConfig.ControllersOutputFile, $"{model.modelName}Controller.cs");
            var ControllersCode = CreateControllersCode(model.modelName, model.Description, codeView.Propertys);
            if (!File.Exists(ControllersPath))
                await File.WriteAllTextAsync(ControllersPath, ControllersCode);

            var IRepositoryPath = Path.Combine(BasePath, templateConfig.IRepositoryOutputFile, $"I{model.modelName}Repository.cs");
            var IRepositoryCode = CreateIRepositoryCode(model.modelName, model.Description);
            if (!File.Exists(IRepositoryPath))
                await File.WriteAllTextAsync(IRepositoryPath, IRepositoryCode);

            var IServicesPath = Path.Combine(BasePath, templateConfig.IServicesOutputFile, $"I{model.modelName}Services.cs");
            var IServicesCode = CreateIServicesCode(model.modelName, model.Description);
            if (!File.Exists(IServicesPath))
                await File.WriteAllTextAsync(IServicesPath, IServicesCode);

            var ServicesPath = Path.Combine(BasePath, templateConfig.ServicesOutputFile, $"{model.modelName}Services.cs");
            var ServicesCode = CreateServicesCode(model.modelName, model.Description);
            if (!File.Exists(ServicesPath))
                await File.WriteAllTextAsync(ServicesPath, ServicesCode);

            var RepositoryPath = Path.Combine(BasePath, templateConfig.RepositoryOutputFile, $"{model.modelName}Repository.cs");
            var RepositoryCode = CreateRepositoryCode(model.modelName, model.Description);
            if (!File.Exists(RepositoryPath))
                await File.WriteAllTextAsync(RepositoryPath, RepositoryCode);
        }
    }



}
