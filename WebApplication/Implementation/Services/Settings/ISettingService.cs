namespace SKBKontur.Treller.WebApplication.Implementation.Services.Settings
{
    public interface ISettingService
    {
        BoardSettings[] GetDevelopingBoards();
        BoardSettings[] GetDevelopingBoardsWithClosed();
    }
}