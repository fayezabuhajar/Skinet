using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specification;

public class BrandListSpecification : BaseSpecification<Product, string>
{
    public BrandListSpecification()
    {
        AddSelect(x => x.Brand);
        ApplayDistinct();
    }
}