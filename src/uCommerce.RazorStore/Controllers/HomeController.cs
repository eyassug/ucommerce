﻿using System.Web.Mvc;
using Umbraco.Web.Mvc;
namespace UCommerce.RazorStore.Controllers
{
	public class HomeController : RenderMvcController
	{
		public ActionResult Index()
		{
			return View("/views/frontpage.cshtml");
		}
	}
}