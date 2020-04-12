using Framework.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.CodeTemplate
{
    public interface ICodeContext
    {
        Task<List<TableInfo>> MysqlGetTablesAsync();

        Task<List<TableFieldInfo>> MysqlGetTableFieldAsync(string TableName);

        CodeView GetTemplateConfig();

        resultCode ShowOutTemplateCode(CodeView codeView);

        Task OutTemplateCode(CodeView codeView);

        List<modelInfo> GetModelInfos(string modelName = "");

        List<modelProperty> GetProperty(string modelName);

    }

}
