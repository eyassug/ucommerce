﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace UCommerce.RazorStore.Models
{
	public class ShippingViewModel: RenderModel
	{
        public ShippingViewModel() : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent)
	    {
	        
	    }

        public IList<SelectListItem> AvailableShippingMethods { get; set; }

		public int SelectedShippingMethodId { get; set; }

        public string ShippingCountry { get; set; }
	}
}