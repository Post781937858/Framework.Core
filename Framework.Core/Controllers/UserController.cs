using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Common;
using Framework.Core.IServices;
using Framework.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Core.Controllers
{
    [Authorize(Permissions.Name)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices userServices;
        private readonly IUser user;

        public UserController(IUserServices userServices,IUser user)
        {
            this.userServices = userServices;
            this.user = user;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<User>>> Query(int Pageindex, int PageSize = 10, string userNumber = "", string powerName = "", int userState = 0)
        {
            Expression<Func<User, bool>> whereExpression = r => true;
            Expression<Func<User, bool>> whereExpression1 = null;
            Expression<Func<User, bool>> whereExpression2 = null;
            Expression<Func<User, bool>> whereExpression3 = null;

            if (!string.IsNullOrEmpty(userNumber))
            {
                whereExpression1 = r => r.UserNumber == userNumber;
                whereExpression = whereExpression.And(whereExpression1);
            }
            if (!string.IsNullOrEmpty(powerName))
            {
                whereExpression2 = r => r.PowerName == powerName;
                whereExpression = whereExpression.And(whereExpression2);
            }
            if (userState != 0)
            {
                whereExpression3 = r => r.UserState == userState;
                whereExpression = whereExpression.And(whereExpression3);
            }
            var data = await userServices.QueryPage(whereExpression, Pageindex, PageSize);
            data.data.ForEach(p => p.Password = "");
            return new MessageModel<PageModel<User>>(data);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> Add(User model)
        {
            model.Id = 0;
            model.CreateTime = DateTime.Now;
            model.Password = MD5Helper.MD5Encrypt32(model.Password);
            return new MessageModel(await userServices.Add(model) > 0);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<MessageModel> Update(User model)
        {
            model.Password = MD5Helper.MD5Encrypt32(model.Password);
            return new MessageModel(await userServices.Update(model));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Listmodel"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<MessageModel> Delete(List<User> Listmodel)
        {
            List<object> Ids = new List<object>();
            Listmodel.ForEach(p => Ids.Add(p.Id));
            return new MessageModel(await userServices.DeleteByIds(Ids.ToArray()));
        }

        /// <summary>
        /// 图像上传
        /// </summary>
        /// <param name="env"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("Icon")]
        public async Task<JsonResult> Post([FromServices]IWebHostEnvironment env, IFormCollection files)
        {
            var previewUrl = string.Empty;
            long fileSize = 0;
            var fileName = string.Empty;
            if (files.Files.Count > 0)
            {
                var uploadFile = files.Files[0];
                var webSiteUrl = "~/images/uploader/UserIcon/";
                var Extension = Path.GetExtension(uploadFile.FileName);
                string[] extension = { ".jpg", ".png",".svg", ".jpeg" };
                if(!extension.Contains(Extension.ToLower()))
                {
                    throw new Exception("文件类型错误");
                }
                fileName = $"{user.Name}{Extension}";
                var filePath = Path.Combine(env.ContentRootPath, webSiteUrl.Replace("~", string.Empty).Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar) + fileName);
                var fileFolder = Path.GetDirectoryName(filePath);
                fileSize = uploadFile.Length;
                if (!Directory.Exists(fileFolder)) Directory.CreateDirectory(fileFolder);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(fs);
                }
                var iconName = $"{fileName}?v={DateTime.Now.Ticks}";
                previewUrl = Url.Content($"{webSiteUrl}{iconName}");
            }
            return new JsonResult(new
            {
                initialPreview = previewUrl,
                initialPreviewConfig = new { caption = "新头像", size = fileSize, key = fileName }
            });
        }
    }
}