﻿using GSRU_API.Services.Implementation;
using GSRU_API.Services.Interfaces;
using GSRU_DataAccessLayer;
using Microsoft.Extensions.DependencyInjection;

namespace GSRU_API.BusinessLayer
{
    public static class BusinessLayerConfiguration
    {
        public static void RegisterBusinessLayerDependencies(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            DataAccessLayerConfiguration.RegisterDependencies(services);
        }
    }
}