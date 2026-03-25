using EsperancaSolidaria.Identity.API.Binders;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EsperancaSolidaria.Identity.API.Filters
{
    public class IdsFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var encryptedIds = context
                .ApiDescription
                .ParameterDescriptions
                .Where(x => x.ModelMetadata.BinderType == typeof(EsperancaSolidariaIdentityBinder))
                .ToDictionary(d => d.Name, d => d);

            if(operation.Parameters is { Count: > 0 })
            {
                for(var i = 0; i < operation.Parameters.Count; i++)
                {
                    var parameter = operation.Parameters[i];

                    if(parameter?.Name is null)
                    {
                        continue;
                    }

                    if(encryptedIds.ContainsKey(parameter.Name))
                    {
                        operation.Parameters[0] = new OpenApiParameter
                        {
                            Name = parameter.Name,
                            In = parameter.In,
                            Description = parameter.Description,
                            Required = parameter.Required,
                            Deprecated = parameter.Deprecated,
                            AllowEmptyValue = parameter.AllowEmptyValue,
                            Style = parameter.Style,
                            Explode = parameter.Explode,
                            Schema = AsStringSchema(parameter.Schema)
                        };
                    }
                }
            }
        }

        static OpenApiSchema AsStringSchema(IOpenApiSchema? old)
        {
            return new OpenApiSchema
            {
                Type = JsonSchemaType.String,
                Format = string.Empty,
                Description = old?.Description,
                Default = old?.Default,
                Example = old?.Example
            };
        }
    }
}