using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.webshop
{
    public class Webshop : IWebshop
    {
        private IBasket basket;

        public Webshop(IBasket basket)
        {
            this.basket = basket;
        }

        public IBasket Basket
        {
            get { return basket; }
            set { basket = value; }
        }

        public void Checkout(IBilling billing)
        {
            if (billing == null)
                throw new ArgumentNullException();

            else if (billing.Balance < basket.TotalCost)
                throw new InsufficientFundsException();

            billing.Pay(basket.TotalCost);          
        }
    }
}
