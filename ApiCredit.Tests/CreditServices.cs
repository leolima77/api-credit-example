using ApiCredit.Models;
using ApiCredit.Services.Interfaces;
using ApiCredit.Services.Repositories;

namespace ApiCredit.Tests
{
    [TestClass]
    public class CreditServiceTests
    {
        private ICreditService _creditService;

        [TestInitialize]
        public void Setup()
        {
            var riskService = new RiskService();
            var digitalKeyService = new MockDigitalKeyService();

            _creditService = new CreditService(riskService, digitalKeyService);
        }

        [TestMethod]
        public void AskForCredit_ShouldApprove_WhenRiskIsLow()
        {
            var result = _creditService.AskForCredit(
                taxId: "123456789",
                unemploymentTax: 2,
                inflation: 1,
                creditHistory: 10,
                debts: 0);

            Assert.IsTrue(result.Approved);
            Assert.IsTrue(result.RiskIndex <= 4);
        }

        [TestMethod]
        public void AskForCredit_ShouldReject_WhenRiskIsHigh()
        {
            var result = _creditService.AskForCredit(
                taxId: "987654321",
                unemploymentTax: 10,
                inflation: 8,
                creditHistory: 3,
                debts: 10000);

            Assert.IsFalse(result.Approved);
            Assert.IsTrue(result.RiskIndex > 4);
        }

        private class MockDigitalKeyService : IDigitalKeyService
        {
            public User GetDataByTaxId(string taxId)
            {
                return new User
                {
                    Name = "Teste",
                    TaxId = taxId,
                    Email = "teste@email.com",
                    MontlyIncome = 1500
                };
            }
        }
    }
}
