using ApiCredit.Services.Interfaces;
using System;

namespace ApiCredit.Services.Repositories
{
    /// <summary>
    /// Service responsible for calculating the customer risk index based on economic and financial factors.
    /// </summary>
    public class RiskService : IRiskService
    {
        /// <summary>
        /// Calculates a risk index based on unemployment rate, inflation, credit history, and debts.
        /// </summary>
        /// <param name="unemploymentTax">Unemployment rate in percentage.</param>
        /// <param name="inflation">Inflation rate in percentage.</param>
        /// <param name="creditHistory">Credit history score (0-10).</param>
        /// <param name="debts">Customer debts in euros.</param>
        /// <returns>A risk index ranging from 0 (low risk) to 10 (high risk).</returns>
        public double CalculateRiskIndex(double unemploymentTax, double inflation, int creditHistory, double debts)
        {
            double risk = 0;

            risk += unemploymentTax * 0.25;
            risk += inflation * 0.2;
            risk += (10 - creditHistory) * 0.3;
            risk += (debts / 1000.0) * 0.25;

            return Math.Clamp(risk, 0, 10);
        }
    }

}
