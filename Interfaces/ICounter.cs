using ProductStructureTypeLib;

namespace TechBOM.Interfaces
{
    public interface ICounter
    {
        void Reset();
        void CountParts(Product oProduct, int currentDepth, string maxDepthStr);
    }
}
