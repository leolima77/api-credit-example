namespace ApiCredit.Services.Interfaces
{
    public interface IDigitalKeyService
    {
        Models.User GetDataByTaxId(string taxId);
    }
}
