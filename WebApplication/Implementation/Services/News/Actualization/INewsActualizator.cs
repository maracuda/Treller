namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Actualization
{
    public interface INewsActualizator
    {
        void ActualizeAll(int batchSize = 30);
    }
}