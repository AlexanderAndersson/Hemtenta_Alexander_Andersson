using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.bank
{
    public class Account : IAccount
    {
        public double Amount { get; set; }

        public void Deposit(double amount)
        {
            bool illegalAmount = double.IsNegativeInfinity(amount) || amount == double.MinValue
                || double.IsPositiveInfinity(amount) || amount <= 0 ;
            bool NaN = double.IsNaN(amount);

            if (illegalAmount)
                throw new IllegalAmountException();

            else if (NaN)
                throw new OperationNotPermittedException();

            else
                Amount += amount;
        }

        public void TransferFunds(IAccount destination, double amount)
        {
            destination.Deposit(amount);
            Amount -= amount;

            if (Amount < 0)
                throw new InsufficientFundsException();
        }

        public void Withdraw(double amount)
        {
            bool illegalAmount = double.IsNegativeInfinity(amount) || amount == double.MinValue 
                || double.IsPositiveInfinity(amount) || amount <= 0;
            bool NaN = double.IsNaN(amount);

            if (illegalAmount)
                throw new IllegalAmountException();

            else if (Amount < amount)
                throw new InsufficientFundsException();

            else if (NaN)
                throw new OperationNotPermittedException();

            else
                Amount -= amount;
        }
    }
}
