using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Core.CodeTemplate;
using Framework.Core.Models;
using Framework.Core.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Core.Extensions
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeCreateController : ControllerBase
    {
        private readonly ICodeContext codeContext;

        public CodeCreateController(ICodeContext codeContext)
        {
            this.codeContext = codeContext;
        }

        [HttpGet]
        public MessageModel<List<modelInfo>> GetTablesAsync(string Name = "")
        {
            return new MessageModel<List<modelInfo>>(codeContext.GetModelInfos());
        }


        [HttpGet("Property")]
        public  MessageModel<List<modelProperty>> GetFieldAsync(string modelName)
        {
            return new MessageModel<List<modelProperty>>(codeContext.GetProperty(modelName));
        }

        [HttpGet("GetTemplateConfig")]
        public MessageModel<CodeView> GetTemplateConfig()
        {
            return new MessageModel<CodeView>(codeContext.GetTemplateConfig());
        }

        [HttpPost]
        public async Task<MessageModel<resultCode>> ShowOutTemplateCodeAsync(CodeView codeView)
        {
            return new MessageModel<resultCode>(await codeContext.ShowOutTemplateCode(codeView));
        }

        [HttpPut]
        public async Task<MessageModel> OutTemplateCodeAsync(CodeView codeView)
        {
            try
            {
                await codeContext.OutTemplateCode(codeView);
                return new MessageModel();
            }
            catch (Exception ex)
            {
                return new MessageModel(false, ex.Message);
            }
        }
    }
}