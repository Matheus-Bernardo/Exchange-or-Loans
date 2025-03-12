namespace ExchangeOrLoans.Utils;

public interface IImageService
{
    Task<string> SaveImage(IFormFile file);
}