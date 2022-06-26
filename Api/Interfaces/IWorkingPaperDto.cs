namespace Api.Interfaces
{
    public interface IWorkingPaperDto
    {
        int Id { get; set; }
        string Title { get; set; }
        string Abstract { get; set; }
        string Link { get; set; }
        string DatasetLink { get; set; }
    }
}