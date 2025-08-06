namespace ApiCredit.Services.Interfaces
{
    public interface IRiskService
    {
        double CalculateRiskIndex(double unemploymentTax, double inflation, int creditHistory, double debts);
    }
}
