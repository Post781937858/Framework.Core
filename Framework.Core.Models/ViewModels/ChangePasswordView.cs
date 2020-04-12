using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Models.ViewModels
{
    public class ChangePasswordView
    {
        public string originalPassword { get; set; }

        public string checkPassword { get; set; }
    }
}
