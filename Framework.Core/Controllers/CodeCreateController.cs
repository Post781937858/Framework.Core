using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Core.CodeTemplate;
using Framework.Core.Models;
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
        public async Task<MessageModel<List<TableInfo>>> GetTablesAsync(string Name = "")
        {
            return new MessageModel<List<TableInfo>>(await codeContext.MysqlGetTablesAsync());
        }


        [HttpGet("Field")]
        public async Task<MessageModel<List<TableFieldInfo>>> GetFieldAsync(string TableName)
        {
            return new MessageModel<List<TableFieldInfo>>(await codeContext.MysqlGetTableFieldAsync(TableName));
        }
    }
}