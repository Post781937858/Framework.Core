using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Framework.Core.CodeTemplate;
using Framework.Core.IRepository.IUnitOfWork;
using Framework.Core.IServices;
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
        private readonly IMenuServices menuServices;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CodeCreateController(ICodeContext codeContext, IMenuServices menuServices, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.codeContext = codeContext;
            this.menuServices = menuServices;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet]
        public MessageModel<List<modelInfo>> GetTablesAsync(string modelName = "")
        {
            return new MessageModel<List<modelInfo>>(codeContext.GetModelInfos(modelName));
        }


        [HttpGet("Property")]
        public MessageModel<List<modelPropertyView>> GetFieldAsync(string modelName)
        {
            return new MessageModel<List<modelPropertyView>>(mapper.Map<List<modelPropertyView>>(codeContext.GetProperty(modelName)));
        }

        [HttpGet("GetTemplateConfig")]
        public MessageModel<CodeView> GetTemplateConfig()
        {
            return new MessageModel<CodeView>(codeContext.GetTemplateConfig());
        }

        [HttpPost]
        public MessageModel<resultCode> ShowOutTemplateCodeAsync(CodeView codeView)
        {
            return new MessageModel<resultCode>( codeContext.ShowOutTemplateCode(codeView));
        }

        [HttpPut]
        public async Task<MessageModel> OutTemplateCodeAsync(CodeView codeView)
        {
            try
            {
                var menus = await menuServices.Query(p => p.url.ToLower() == codeView.url || p.title == codeView.title);
                if (!menus.Any())
                {
                  var result= await menuServices.Add(new Menu()
                    {
                        menuid = codeView.menuId == 0 ? 999 : codeView.menuId,
                        title = codeView.title,
                        explain = codeView.description,
                        icon = codeView.icon,
                        menutype = menuType.Menu,
                        method = "#",
                        state = 200,
                        no = codeView.sort,
                        url = codeView.url
                    });
                    var menu = (await menuServices.Query(p => p.url.ToLower() == codeView.url.ToLower() && p.title == codeView.title)).FirstOrDefault();
                    if (menu != null)
                    {
                        unitOfWork.BeginTran();
                        var ListMenu = new List<Menu>();
                        for (int i = 0; i < 4; i++)
                        {
                            var menutitle = string.Empty;
                            var menumethod = string.Empty;
                            if (i == 0)
                            {
                                menumethod = "get";
                                menutitle = $"{ codeView.title}-查询";
                            }
                            else if (i == 1)
                            {
                                menumethod = "put";
                                menutitle = $"{ codeView.title}-修改";
                            }
                            else if (i == 2)
                            {
                                menumethod = "post";
                                menutitle = $"{ codeView.title}-添加";
                            }
                            else if (i == 3)
                            {
                                menumethod = "delete";
                                menutitle = $"{ codeView.title}-删除";
                            }
                            ListMenu.Add(new Menu()
                            {
                                menuid = menu.Id,
                                title = menutitle,
                                explain = "",
                                icon = codeView.icon,
                                menutype = menuType.Button,
                                method = menumethod,
                                state = 200,
                                no = codeView.sort,
                                url = $"/api/{codeView.model.modelName}"
                            });
                        }
                        await menuServices.Add(ListMenu);
                        unitOfWork.CommitTran();
                        await codeContext.OutTemplateCode(codeView);
                        return new MessageModel();
                    }
                    else
                    {
                        return new MessageModel(false, "失败");
                    }
                }
                return new MessageModel(false, "已存在");
            }
            catch (Exception ex)
            {
                unitOfWork.RollbackTran();
                return new MessageModel(false, ex.Message);
            }
        }
    }
}