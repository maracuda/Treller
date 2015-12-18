namespace SKBKontur.Treller.WebApplication.Services.Settings
{
    public interface ISettingService
    {
        string[] GetDevelopingBoardIds();
        BoardSettings[] GetDevelopingBoards();
        BoardSettings[] GetDevelopingBoardsWithClosed();
    }
}