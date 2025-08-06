using ApiCredit.Services.Interfaces;

namespace ApiCredit.Services.Repositories
{
    /// <summary>
    /// Service responsible for simulating data retrieval from the Digital Key system using the Portuguese Tax ID.
    /// </summary>
    public class DigitalKeyServices : IDigitalKeyService
    {
        /// <summary>
        /// Simulates fetching user data from a trusted national identity source based on the provided tax ID.
        /// </summary>
        /// <param name="taxId">Portuguese Tax Identification Number (NIF).</param>
        /// <returns>User object populated with mock data.</returns>
        public Models.User GetDataByTaxId(string taxId)
        {
            return new Models.User
            {
                Name = "João Silva",
                TaxId = taxId,
                Email = "joao.silva@email.com",
                MontlyIncome = 1800
            };
        }
    }

}
