using Framework.Core.Common;
using System;
using System.IO;

namespace Framework.Core.CodeTemplate
{
    public class TemplateConfig
    {
        private string AppDomainFilePath;

        public TemplateConfig()
        {
            AppDomainFilePath = AppDomain.CurrentDomain.BaseDirectory;
        }


        #region  模板文件路径

        public string ModelsTemplateFile
        {
            get
            {
                return Path.Combine(AppDomainFilePath, @"template\model.tpl");
            }
        }

        public string ControllersTemplateFile
        {
            get
            {
                return Path.Combine(AppDomainFilePath, @"template\Controllers.tpl");
            }
        }

        public string VueTemplateFile
        {
            get
            {
                return Path.Combine(AppDomainFilePath, @"template\Vue.tpl");
            }
        }

        public string IServicesTemplateFile
        {
            get
            {
                return Path.Combine(AppDomainFilePath, @"template\IServices.tpl");
            }
        }

        public string IRepositoryTemplateFile
        {
            get
            {
                return Path.Combine(AppDomainFilePath, @"template\IRepository.tpl");
            }
        }

        public string RepositoryTemplateFile
        {
            get
            {
                return Path.Combine(AppDomainFilePath, @"template\Repository.tpl");
            }
        }


        public string ServicesTemplateFile
        {
            get
            {
                return Path.Combine(AppDomainFilePath, @"template\Services.tpl");
            }
        }

        #endregion

        #region 映射输出目录


        public string VueOutputFile
        {
            get
            {
                return Appsettings.app(new string[] { "CodeTemplate", "VueOutputFile" });
            }
        }

        public string ControllersOutputFile
        {
            get
            {
                return Appsettings.app(new string[] { "CodeTemplate", "ControllersOutputFile" });
            }
        }

        public string ModelsOutputFile
        {
            get
            {
                return Appsettings.app(new string[] { "CodeTemplate", "ModelsOutputFile" });
            }
        }

        public string IServicesOutputFile
        {
            get
            {
                return Appsettings.app(new string[] { "CodeTemplate", "IServicesOutputFile" });
            }
        }

        public string IRepositoryOutputFile
        {
            get
            {
                return Appsettings.app(new string[] { "CodeTemplate", "IRepositoryOutputFile" });
            }
        }

        public string RepositoryOutputFile
        {
            get
            {
                return Appsettings.app(new string[] { "CodeTemplate", "RepositoryOutputFile" });
            }
        }


        public string ServicesOutputFile
        {
            get
            {
                return Appsettings.app(new string[] { "CodeTemplate", "ServicesOutputFile" });
            }
        }

        #endregion

    }
}
