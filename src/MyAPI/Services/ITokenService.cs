namespace MyAPI.Services
{
    public interface ITokenService
    {
        string CreateToken(List<string> rolesList);

        Task<bool> ValidateToke(string token);
    }
}
