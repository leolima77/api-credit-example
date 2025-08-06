using ApiCredit.Models;
using ApiCredit.Services.Interfaces;

namespace ApiCredit.Services.Repositories
{
    /// <summary>
    /// Service responsible for processing credit requests and evaluating customer risk.
    /// </summary>
    public class CreditService : ICreditService
    {
        private readonly IRiskService _riskService;
        private readonly IDigitalKeyService _digitalKeyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditService"/> class.
        /// </summary>
        /// <param name="riskService">The service that calculates customer risk index.</param>
        /// <param name="digitalKeyService">The service that retrieves user data using Tax ID.</param>
        public CreditService(IRiskService riskService, IDigitalKeyService digitalKeyService)
        {
            _riskService = riskService;
            _digitalKeyService = digitalKeyService;
        }

        /// <summary>
        /// Processes a credit request and returns the risk analysis result.
        /// </summary>
        /// <param name="taxId">Tax Identification Number (NIF).</param>
        /// <param name="unemploymentTax">Unemployment rate.</param>
        /// <param name="inflation">Inflation rate.</param>
        /// <param name="creditHistory">Customer credit score.</param>
        /// <param name="debts">Total debts of the customer.</param>
        /// <returns>A response indicating risk index and approval decision.</returns>
        public CreditAnalysisResponse AskForCredit(string taxId, double unemploymentTax, double inflation, int creditHistory, double debts)
        {
            User user = _digitalKeyService.GetDataByTaxId(taxId);

            double risk = _riskService.CalculateRiskIndex(unemploymentTax, inflation, creditHistory, debts);

            return new CreditAnalysisResponse()
            {
                RiskIndex = risk,
                Approved = risk <= 4
            };
        }
    }

}
