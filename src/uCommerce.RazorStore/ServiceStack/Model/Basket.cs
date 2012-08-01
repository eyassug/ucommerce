using System.Collections.Generic;
using uCommerce.RazorStore.ServiceStack.Commands;

namespace uCommerce.RazorStore.ServiceStack.Model
{
    public class Basket
    {
        public decimal? OrderTotal { get; set; }

        public decimal? SubTotal { get; set; }

        public List<LineItem> LineItems { get; set; }
    }
}