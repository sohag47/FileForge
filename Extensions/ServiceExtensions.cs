using FileForge.Entities.Base;
using FileForge.Infrastructure;
using FileForge.Interfaces;
using FileForge.Repositories;
using FileForge.Services;
using FluentValidation;
//using FileForge.Service;
//using FileForge.Service.Category;
//using FileForge.Validators;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace FileForge.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ICategoryService, CategoryService>();

        //services.AddScoped<IFileUploadService, FileUploadService>();
        //services.AddScoped<IJwtTokenService, JwtTokenService>();
        //services.AddScoped<IProductService, ProductService>();
        //services.AddScoped<IUserService,  UserService>();

        // Register Validators
        services.AddValidatorsFromAssembly(typeof(ServiceExtensions).Assembly);

        return services;
    }

    public static IServiceCollection AddValidationConfigurations(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation(options => options.OverrideDefaultResultFactoryWith<CustomValidationResultFactory>());



        // Custom API response for validation errors (Standard DataAnnotations)
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value!.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                var response = ApiResponse<string>.Fail("Validation failed.", errors);

                return new UnprocessableEntityObjectResult(response);
            };
        });

        return services;
    }
}
