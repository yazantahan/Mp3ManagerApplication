namespace MP3ManagerApplication.Pages
{
    interface IUserInterface
    {
        void Run(ref MP3Engine mp3Engine);
        void Dispose();
    }
}
