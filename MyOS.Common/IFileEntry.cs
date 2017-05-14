namespace MyOS.Common
{
    public interface IFileEntry
    {
        string Name { get; set; }
        long Position { get; set; }
        long Length { get; set; }
    }
}