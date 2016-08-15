﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCommerce.Api;
using UCommerce.EntitiesV2;
using UCommerce.EntitiesV2.Queries.Marketing;
using UCommerce.Extensions;
using UCommerce.RazorStore.Models;
using UCommerce.Runtime;
using UCommerce.Search.Indexers;
using Umbraco.Web.Mvc;

namespace UCommerce.MasterClass.Website.Controllers
{
    public class CategoryController : RenderMvcController
    {
        public ActionResult Index()
        {
            var categoryViewModel = new CategoryViewModel();

            var currentCategory = SiteContext.Current.CatalogContext.CurrentCategory;

            categoryViewModel.Name = currentCategory.DisplayName();
            categoryViewModel.Description = currentCategory.Description();
            //new
            categoryViewModel.Products = MapProducts(CatalogLibrary.GetProducts(currentCategory));

            var productsInCategory = UCommerce.Api.CatalogLibrary.GetProducts(currentCategory);

            categoryViewModel.Products = MapProducts(productsInCategory);

            return View("/views/category.cshtml", categoryViewModel);
        }

        private IList<ProductViewModel> MapProducts(ICollection<Product> productsInCategory)
        {
            IList<ProductViewModel> productViews = new List<ProductViewModel>();

            foreach (var product in productsInCategory)
            {
                var productViewModel = new ProductViewModel();

                productViewModel.Sku = product.Sku;
                productViewModel.Name = product.DisplayName();
                productViewModel.Url = "/store/product?product=" + product.ProductId;

                productViewModel.PriceCalculation = CatalogLibrary.CalculatePrice(product);

                productViews.Add(productViewModel);
            }

            return productViews;
        }
    }
}