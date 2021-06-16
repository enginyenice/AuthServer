﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Extensions
{
    public static class CustomValidationResponse
    {
        public static void UseCustomValidationResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {

                    var errors = context.ModelState.Values
                    .Where(p => p.Errors.Count > 0)
                    .SelectMany(p => p.Errors)
                    .Select(p => p.ErrorMessage);

                    ErrorDto errorDto = new ErrorDto(errors.ToList(), true);
                    var response = Response<NoDataDto>.Fail(errorDto, 400);
                    return new BadRequestObjectResult(response);
                };
            });
        }
    }
}
