using Felix.Identity.Api.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Identity.Api.Validation
{
    public class UserInfoModelValidation : AbstractValidator<UserInfoModel>
    {
        public UserInfoModelValidation()
        {
            RuleFor(x => x).NotNull().WithMessage("Invalid user");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Invalid user");
        }
    }
}
