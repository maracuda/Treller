namespace SKBKontur.Treller.WebApplication.Implementation.Services.Settings
{
    public interface ISettingService
    {
        string[] GetDevelopingBoardIds();
        BoardSettings[] GetDevelopingBoards();
        BoardSettings[] GetDevelopingBoardsWithClosed();
    }
}