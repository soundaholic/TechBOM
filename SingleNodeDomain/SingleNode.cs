using INFITF;

namespace TechBOM.SingleNodeDomain
{
    public class SingleNode
    {
        private readonly NodeExtractor _extractor;
        private readonly NodeValidator _validator;
        public NodeData Data { get; private set; }
        public bool IsRoot { get; private set; }

        public SingleNode(Document document)
        {
            _extractor = new NodeExtractor();
            _validator = new NodeValidator();

            // Extract data
            Data = _extractor.Extract(document);

            // Validate node
            IsRoot = NodeValidator.IsRoot(document);
        }
    }

}
