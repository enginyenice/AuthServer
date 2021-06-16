using AuthServer.Core.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.API.Validation
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(p => p.Email).NotEmpty().EmailAddress();
            RuleFor(p => p.Password).NotEmpty();
            RuleFor(p => p.UserName).NotEmpty();
        }
    }
}
