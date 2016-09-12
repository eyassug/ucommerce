﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UCommerce.Api;
using UCommerce.Content;
using UCommerce.EntitiesV2;
using Umbraco.Web.Mvc;
using UCommerce.Extensions;
using UCommerce.Infrastructure;
using UCommerce.RazorStore.Models;
using UCommerce.RazorStore.Services.Commands;
using UCommerce.Runtime;
using UCommerce.Search.Facets;
using Umbraco.Web.Models;

namespace UCommerce.RazorStore.Controllers
{
    public class CategoryController : RenderMvcController
    {
        //public ActionResult Index(RenderModel model)
        //{
        //    var categoryViewModel = new CategoryViewModel();
        //    var currentCategory = SiteContext.Current.CatalogContext.CurrentCategory;

        //    categoryViewModel.Name = currentCategory.DisplayName();
        //    categoryViewModel.Description = currentCategory.Description();

        //    if (!HasBannerImage(currentCategory))
        //    {
        //        var media = ObjectFactory.Instance.Resolve<IImageService>().GetImage(currentCategory.ImageMediaId).Url;
        //        categoryViewModel.BannerImageUrl = media;
        //    }

        //    IList<Facet> facetsForQuerying = System.Web.HttpContext.Current.Request.QueryString.ToFacets();
        //    var productRepository = ObjectFactory.Instance.Resolve<IRepository<Product>>();
        //    var productsForMapping = new List<Product>();

        //    if (currentCategory.Categories.Any())
        //    {
        //        productsForMapping = GetSubcategoryProducts(currentCategory.Categories, facetsForQuerying);
        //    }
        //    else
        //    {

        //List<int> productsInCategory = SearchLibrary.GetProductsFor(currentCategory, facetsForQuerying).Select(x => x.Id).ToList();
        //productsForMapping = productRepository.Select(x => productsInCategory.Contains(x.ProductId)).ToList();
        //    }

        //    categoryViewModel.Products = MapProducts(productsForMapping);
        //    return base.View("/Views/Catalog.cshtml", categoryViewModel);
        //}

        public ActionResult Index(RenderModel model)
        {
            var categoryViewModel = new CategoryViewModel();
            var currentCategory = SiteContext.Current.CatalogContext.CurrentCategory;

            categoryViewModel.Name = currentCategory.DisplayName();
            categoryViewModel.Description = currentCategory.Description();
            categoryViewModel.CatalogId = currentCategory.ProductCatalog.Id;
            categoryViewModel.CategoryId = currentCategory.Id;

            if (!HasBannerImage(currentCategory))
            {
                var media = ObjectFactory.Instance.Resolve<IImageService>().GetImage(currentCategory.ImageMediaId).Url;
                categoryViewModel.BannerImageUrl = media;
            }

            IList<Facet> facetsForQuerying = System.Web.HttpContext.Current.Request.QueryString.ToFacets();

            categoryViewModel.Products = MapProducts(SearchLibrary.GetProductsFor(currentCategory, facetsForQuerying));

            categoryViewModel.Products = MapProducts(currentCategory);
            return base.View("/Views/Catalog.cshtml", categoryViewModel);
        }


        private List<Product> GetSubcategoryProducts(ICollection<Category> subcategories, IList<Facet> facetsForQuerying)
        {
            var productsforMapping = new List<Product>();
            foreach (var subcategory in subcategories)
            {
                List<int> productsInCategory =
                    SearchLibrary.GetProductsFor(subcategory, facetsForQuerying).Select(x => x.Id).ToList();
                var productRepository = ObjectFactory.Instance.Resolve<IRepository<Product>>();
                productsforMapping.AddRange(
                    productRepository.Select(x => productsInCategory.Contains(x.ProductId)).ToList());
            }
            return productsforMapping;
        }

        private bool HasBannerImage(Category category)
        {
            return string.IsNullOrEmpty(category.ImageMediaId);
        }

        private IList<ProductViewModel> MapProducts(ICollection<Documents.Product> productsInCategory)
        {
            IList<ProductViewModel> productViews = new List<ProductViewModel>();

            foreach (var product in productsInCategory)
            {
                var productViewModel = new ProductViewModel();

                productViewModel.Sku = product.Sku;
                productViewModel.Name = product.Name;
                productViewModel.ThumbnailImageUrl = product.ThumbnailImageUrl;

                productViews.Add(productViewModel);
            }

            return productViews;
        }

        private List<ProductViewModel> MapProducts(Category category)
        {
            var productsInCategory = new List<ProductViewModel>();
            if (!category.Categories.Any())
            {
                var parentCategoryProducts = MapProducts(category.Products.ToList());
                productsInCategory.AddRange(parentCategoryProducts);
            }
            else
            {
                foreach (var subcategory in category.Categories)
                {
                    MapProducts(subcategory);
                }
            }
            return productsInCategory;
        }

    }
}