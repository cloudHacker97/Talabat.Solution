﻿using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories;
using Talabat.Repository;

namespace Talabat.APIs.Extensions
{
	public static class  ApplicationServiceExtension
	{

		public static IServiceCollection AddApplicationService(this IServiceCollection services)
		{
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
			services.Configure<ApiBehaviorOptions>(Options =>
			{
				Options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
																					.SelectMany(P => P.Value.Errors)
																					.Select(E => E.ErrorMessage)
																					.ToList();
					var response = new ApiValidationErrorResponse()
					{ Errors = errors };
					return new BadRequestObjectResult(response);
				};

			});
			return services;
		}
	}
}
