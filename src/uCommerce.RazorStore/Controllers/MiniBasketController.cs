﻿using System.Linq;
using System.Web.Mvc;
using UCommerce.Api;
using UCommerce.EntitiesV2;
using Umbraco.Web.Mvc;
using UCommerce.RazorStore.Models;

namespace UCommerce.RazorStore.Controllers
{
    public class MiniBasketController : SurfaceController
    {
// GET: MiniBasket
        public ActionResult Index()
        {
            var miniBasket = new MiniBasketViewModel();
            miniBasket.IsEmpty = true;

            if (TransactionLibrary.HasBasket())
            {
                PurchaseOrder basket = TransactionLibrary.GetBasket(false).PurchaseOrder;
                var numberOfItems = basket.OrderLines.Sum(x => x.Quantity);
                if (numberOfItems != 0)
                {
                    miniBasket.Total = new Money(basket.OrderTotal.GetValueOrDefault(), basket.BillingCurrency);
                    miniBasket.NumberOfItems = basket.OrderLines.Sum(x => x.Quantity);
                    miniBasket.IsEmpty = false;
                }
            }
            return View("/Views/PartialView/MiniBasket.cshtml", miniBasket);
        }
    }
}
    