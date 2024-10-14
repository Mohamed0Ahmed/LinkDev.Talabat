using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.DTOs.Products;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Mapping
{
    internal class ProductPictureResolver(IConfiguration configuration) : IValueResolver<Product, ProductDisplayDto, string?>
    {
        public string? Resolve(Product source, ProductDisplayDto destination, string? destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{configuration["Urls:ApiBaseUrl"]}/{source.PictureUrl}";

            return string.Empty ;
        }
    }
}
