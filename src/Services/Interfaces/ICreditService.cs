using ApiCredit.Models;

namespace ApiCredit.Services.Interfaces
{
    public interface ICreditService
    {
        CreditAnalysisResponse AskForCredit(string taxId, double unemploymentTax, double inflation, int creditHistory, double debts);
    }
}
