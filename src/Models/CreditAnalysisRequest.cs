namespace ApiCredit.Models
{
    /// <summary>
    /// Represents a credit analysis request with personal and financial data.
    /// </summary>
    public class CreditAnalysisRequest
    {
        /// <summary>
        /// Portuguese Tax Identification Number (NIF).
        /// </summary>
        public string TaxId { get; set; }

        /// <summary>
        /// Current unemployment rate (percentage).
        /// </summary>
        public double UnemploymentTax { get; set; }

        /// <summary>
        /// Current inflation rate (percentage).
        /// </summary>
        public double Inflation { get; set; }

        /// <summary>
        /// Customer's credit history score (0 to 10).
        /// </summary>
        public int CreditHistory { get; set; }

        /// <summary>
        /// Total outstanding debts of the customer (in euros).
        /// </summary>
        public double Debts { get; set; }
    }

}
