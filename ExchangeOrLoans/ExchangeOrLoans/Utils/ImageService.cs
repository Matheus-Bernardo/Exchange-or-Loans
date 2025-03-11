namespace ExchangeOrLoans.Utils;

public class ImageService : IImageService
{
    private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "ImagesProduct");

    public ImageService()
    {
        if(!Directory.Exists(_uploadPath)) Directory.CreateDirectory(_uploadPath);
    }
    
    public async Task<string> SaveImage(IFormFile image)   
    {
        if(image is null || image.Length == 0) throw new ArgumentException("Image invalid");
        
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
        var filePath = Path.Combine(_uploadPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            image.CopyTo(stream);
        }
        return $"/ImagesProduct/{fileName}";
    }
}