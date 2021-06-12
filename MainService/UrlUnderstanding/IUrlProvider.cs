namespace MainService.UrlUnderstanding
{
    public interface IUrlProvider
    {
        UrlResult Extract(string url);
    }
}