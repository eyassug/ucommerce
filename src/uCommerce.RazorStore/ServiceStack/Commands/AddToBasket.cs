﻿using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using UCommerce.Api;
using UCommerce.Transactions;

namespace uCommerce.RazorStore.ServiceStack.Commands
{
    public class AddToBasket
    {
        public string CatalogName { get; set; }
        public int Quantity { get; set; }
        public string Sku { get; set; }
        public string VariantSku { get; set; }
        public bool AddToExistingLine { get; set; }
    }

    public class AddToBasketResponse : IHasResponseStatus
    {
        public ResponseStatus ResponseStatus { get; set; }
    }
    
    public class AddToBasketService : ServiceBase<AddToBasket>
    {
        protected override object Run(AddToBasket request)
        {
            //TODO: catalogname is not used - refactor to use catalogId instead
            TransactionLibrary.AddToBasket(request.Quantity,request.Sku,request.VariantSku, request.AddToExistingLine);
            TransactionLibrary.ExecuteBasketPipeline();
            return new AddToBasketResponse();
        }
    }
}