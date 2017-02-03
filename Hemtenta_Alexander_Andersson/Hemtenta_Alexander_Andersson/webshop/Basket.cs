using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.webshop
{
    public class Basket : IBasket
    {
        List<Product> ListOfProducts()
        {
            {
                return productList;
            }
        }

        private List<Product> productList = new List<Product>();
        private decimal totalCost;

        public decimal TotalCost
        {
            get { return totalCost; }
            private set { totalCost = value; }
        }

        public void AddProduct(Product p, int amount)
        {
            if (p == null)
                throw new ArgumentNullException();

            else if (amount <= 0 || p.Price <= 0)
                throw new ProductValueIsNegativeOrZeroException();

            else
            {
                for (int i = 0; i < amount; i++)
                {
                    productList.Add(p);
                    totalCost += p.Price;
                }
            }
        }

        public void RemoveProduct(Product p, int amount)
        {
            if (p == null)
                throw new ArgumentNullException();

            else if (amount <= 0)
                throw new ProductValueIsNegativeOrZeroException();

            else
            {
                for (int i = 0; i < amount; i++)
                {
                    productList.Remove(p);
                    totalCost -= p.Price;
                }
            }
        }
    }
}
