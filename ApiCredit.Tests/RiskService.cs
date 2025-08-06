using ApiCredit.Services.Repositories;

namespace ApiCredit.Tests
{
    [TestClass]
    public class RiskServiceTests
    {
        private RiskService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new RiskService();
        }

        [TestMethod]
        public void CalculateRiskIndex_ShouldReturnLowRisk_WhenInputsAreFavorable()
        {
            var risk = _service.CalculateRiskIndex(unemploymentTax: 2, inflation: 1.5, creditHistory: 10, debts: 0);

            Assert.IsTrue(risk < 4);
        }

        [TestMethod]
        public void CalculateRiskIndex_ShouldReturnModerateRisk_WhenInputsAreAverage()
        {
            var risk = _service.CalculateRiskIndex(unemploymentTax: 6, inflation: 3, creditHistory: 7, debts: 3000);

            Assert.IsTrue(risk >= 3 && risk <= 5);
        }

        [TestMethod]
        public void CalculateRiskIndex_ShouldReturnHighRisk_WhenInputsAreCritical()
        {
            var risk = _service.CalculateRiskIndex(unemploymentTax: 10, inflation: 9, creditHistory: 2, debts: 10000);

            Assert.IsTrue(risk > 7);
        }

        [TestMethod]
        public void CalculateRiskIndex_ShouldClampRiskBetweenZeroAndTen()
        {
            var risk = _service.CalculateRiskIndex(unemploymentTax: 100, inflation: 100, creditHistory: 0, debts: 100000);
            Assert.IsTrue(risk <= 10);

            risk = _service.CalculateRiskIndex(unemploymentTax: -10, inflation: -5, creditHistory: 12, debts: -1000);
            Assert.IsTrue(risk >= 0);
        }
    }
}
