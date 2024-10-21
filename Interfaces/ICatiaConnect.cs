
namespace TechBOM.Interfaces
{
    public interface ICatiaConnect
    {
        INFITF.Application ConnectCatia();
        bool IsDocumentLoaded();
    }
}
