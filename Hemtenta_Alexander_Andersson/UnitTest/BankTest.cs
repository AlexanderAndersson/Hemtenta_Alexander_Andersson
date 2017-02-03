using NUnit.Framework;
using HemtentaTdd2017.bank;
using HemtentaTdd2017;
using System;
using Moq;

namespace UnitTest
{
    [TestFixture]
    public class BankTest
    {
        private Account a;
        private Account RegularAccount;
        private Account SavingsAccount;

        [SetUp]
        public void SetUp()
        {
            a = new Account();
            RegularAccount = new Account();
            SavingsAccount = new Account();
        }

        [Test]
        [TestCase(double.NaN)]
        public void Deposit_IncorrectValues_Throws_NaN(double amount)
        {
            Assert.Throws<OperationNotPermittedException>(() => a.Deposit(amount), "amount is NaN");
        }

        [Test]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.MinValue)]
        [TestCase(0)]
        [TestCase(-0.0001)]
        public void Deposit_IncorrectValues_Throw(double amount)
        {
            Assert.Throws<IllegalAmountException>(() => a.Deposit(amount));
        }

        [Test]
        [TestCase(double.MaxValue)]
        [TestCase(double.Epsilon)] //Eftersom jag inte sätter något minimum värde, så är allt som är mer än noll och mindre än maxValue godkännt
        [TestCase(1)]
        public void Deposit_Success(double amount)
        {
            a.Deposit(amount);
        }

        [Test]
        [TestCase(double.NaN)]
        public void Withdrawl_IncorrectValues_Throw_NaN(double amount)
        {
            Assert.Throws<OperationNotPermittedException>(() => a.Withdraw(amount), "Amount is NaN");
        }

        [Test]
        [TestCase(double.MaxValue)]
        [TestCase(101)]
        public void Withdrawl_IncorrectValues_Throw_NotEnoughMoney(double amount)
        {
            a.Deposit(100);
            Assert.Throws<InsufficientFundsException>(() => a.Withdraw(amount));
        }

        [Test]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.MinValue)]
        [TestCase(0)]
        [TestCase(-0.0001)]
        public void Withdrawl_IncorrectValues_Throws(double amount)
        {
            a.Deposit(100);
            Assert.Throws<IllegalAmountException>(() => a.Withdraw(amount));
        }

        [Test]
        [TestCase(double.Epsilon)]
        [TestCase(1)]
        public void Withdrawl_Success(double amount)
        {
            a.Deposit(100);
            a.Withdraw(amount);
        }

        [Test]
        [TestCase(150, 100)]
        public void TransferFunds_Success(double amountToDeposit, double amountToTransfer)
        {
            RegularAccount.Deposit(amountToDeposit);
            RegularAccount.TransferFunds(SavingsAccount, amountToTransfer);

            Assert.AreEqual(amountToTransfer, SavingsAccount.Amount);
            Assert.AreEqual(amountToDeposit - amountToTransfer, RegularAccount.Amount);
        }

        [Test]
        [TestCase(double.NaN)]
        public void TransferFunds_IncorrectValues_Throw_NaN(double amountToTransfer)
        {
            RegularAccount.Deposit(100);
            Assert.Throws<OperationNotPermittedException>(() => RegularAccount.TransferFunds(SavingsAccount, amountToTransfer), "Amount is NaN");
        }

        [Test]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.MinValue)]
        public void TransferFunds_IncorrectValues_Throws(double amountToTransfer)
        {
            RegularAccount.Deposit(100);
            Assert.Throws<IllegalAmountException>(() => RegularAccount.TransferFunds(SavingsAccount, amountToTransfer));
        }

        [Test]
        [TestCase(150)]
        public void TransferFunds_IncorrectValues_Throw_NotEnoughMoney(double amountToTransfer)
        {
            RegularAccount.Deposit(100);
            Assert.Throws<InsufficientFundsException>(() => RegularAccount.TransferFunds(SavingsAccount, amountToTransfer));
        }
    }
}
