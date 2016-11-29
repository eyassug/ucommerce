﻿using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using UCommerce.Search.Facets;
using Umbraco.Web.Mvc;
using System.Collections.Specialized;
using System.Linq;
using System;
using UCommerce.Infrastructure;
using UCommerce.Publishing.Model;
using UCommerce.Publishing.Runtime;
using UCommerce.RazorStore.Models;
using ICatalogContext = UCommerce.Publishing.Runtime.ICatalogContext;

namespace UCommerce.RazorStore.Controllers
{
    public static class FacetedQueryStringExtensions
    {
        public static IList<Facet> ToFacets(this NameValueCollection target)
        {
            var parameters = new Dictionary<string, string>();
            foreach (var queryString in HttpContext.Current.Request.QueryString.AllKeys)
            {
                parameters[queryString] = HttpContext.Current.Request.QueryString[queryString];
            }
            if (parameters.ContainsKey("umbDebugShowTrace"))
            {
                parameters.Remove("umbDebugShowTrace");
            }
            if (parameters.ContainsKey("product"))
            {
                parameters.Remove("product");
            }
            if (parameters.ContainsKey("category"))
            {
                parameters.Remove("category");
            }
            if (parameters.ContainsKey("catalog"))
            {
                parameters.Remove("catalog");
            }
            var facetsForQuerying = new List<Facet>();

            foreach (var parameter in parameters)
            {
                var facet = new Facet();
                facet.FacetValues = new List<FacetValue>();
                facet.Name = parameter.Key;
                foreach (var value in parameter.Value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    facet.FacetValues.Add(new FacetValue() { Value = value});
                }
                facetsForQuerying.Add(facet);
            }

            return facetsForQuerying;
        }

    }

    public class FacetsController : SurfaceController
    {
        // GET: Facets
        public ActionResult Index()
        {
            var catalogContext = ObjectFactory.Instance.Resolve<ICatalogContext>();
            var catalogLibrary = ObjectFactory.Instance.Resolve<ICatalogLibrary>();

            var category = catalogContext.CurrentCategory;
            var facetValueOutputModel = new FacetsDisplayedViewModel();
            IList<Facet> facetsForQuerying = System.Web.HttpContext.Current.Request.QueryString.ToFacets();

            if (ShouldDisplayFacets(category))
            {
                IList<Facet> facets = catalogLibrary.GetFacetsFor(category.Guid, facetsForQuerying);
                if (facets.Any(x => x.FacetValues.Any(y => y.Hits > 0)))
                {
                    facetValueOutputModel.Facets = MapFacets(facets);
                }
            }

            return View("/Views/PartialView/Facets.cshtml", facetValueOutputModel);
        }

        private bool ShouldDisplayFacets(Category category)
        {
            var catalogContext = ObjectFactory.Instance.Resolve<ICatalogContext>();
            var product = catalogContext.CurrentProduct;

            return category != null && product == null;
        }

        private IList<FacetViewModel> MapFacets(IList<Facet> facetsInCategory)
        {
            IList<FacetViewModel> facets = new List<FacetViewModel>();

            foreach (var facet in facetsInCategory)
            {
                var facetViewModel = new FacetViewModel();
                facetViewModel.Name = facet.Name;
                facetViewModel.DisplayName = facet.DisplayName;

                if (!facet.FacetValues.Any())
                {
                    continue;
                }

                foreach (var value in facet.FacetValues)
                {
                    if (value.Hits > 0)
                    {
                        FacetValueViewModel facetVal = new FacetValueViewModel(value.Value, value.Hits);
                        facetViewModel.FacetValues.Add(facetVal);
                    }
                }

                facets.Add(facetViewModel);
            }

            return facets;
        }
    }
}

