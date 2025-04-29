using INFITF;
using NLog;
using ProductStructureTypeLib;
using TechBOM.SingleNodeDomain;

namespace TechBOM
{
    public class CatiaProcessor
    {
        public List<string> Names { get; set; } = [];
        public List<string> DrawingNumbers { get; set; } = [];
        public List<SingleNode> Nodes { get; set; } = [];
        
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private bool _isActive;
        private bool _isAdapter;
        private bool _isCorrect;

        private int _count = 0;

        public void CatHelperReset()
        {
            Names.Clear();
            Nodes.Clear();
        }

        public void WalkDownTree(Product oInProduct, int currentDepth, string maxDepthStr)
        {
            Document? oDoc = null;

            try
            {
                oDoc = (Document)oInProduct.ReferenceProduct.Parent;
            }
            catch
            {
                _logger.Info($"Unable to retrieve the activity status of Product {oInProduct.get_Name()}");
            }

            SingleNode singleNode = new(oDoc);

            string name = singleNode.Data.PartNumber;
            string oInProductName = oInProduct.get_Name();

            try
            {
                if (_count == 0)
                {
                    _isActive = true;
                    _isAdapter = false;
                }
                else
                {
                    _isActive = NodeValidator.IsActive(oInProduct);
                    _isAdapter = NodeValidator.IsAdapter(singleNode);
                }
            }
            catch
            {
                _logger.Info($"Unable to retrieve the activity status of Product {singleNode.Data.Name}");
            }

            bool isUBPart = NodeValidator.IsUb(singleNode);
            bool _isCorrect = NodeValidator.IsCorrectStartModel(singleNode);
            bool isNameUnique = !Names.Contains(singleNode.Data.DrawingNumber) || !Names.Contains(singleNode.Data.PosNumber);
            bool isRoot = singleNode.IsRoot;
            bool isVariablePosNumber = singleNode.Data.PosNumber == "XXXXX";
            bool isComponent = NodeValidator.IsComponent(oInProduct);
            bool isAspPart = singleNode.Data.PosNumber == "00000";

            bool canAddNode = _isCorrect && isNameUnique && !isRoot && !isUBPart && _isActive && !isVariablePosNumber && !_isAdapter && !isComponent && !isAspPart;

            bool unlimitedDepth = maxDepthStr == "All";

            if (!unlimitedDepth)
            {
                if (!int.TryParse(maxDepthStr, out int maxDepth))
                {
                    throw new ArgumentException("Invalid depth value. It must be a number or 'All'.");
                }

                maxDepth += 1;

                if (currentDepth >= maxDepth)
                {
                    return;
                }
            }

            if (canAddNode)
            {
                Nodes.Add(singleNode);
                Names.Add(singleNode.Data.DrawingNumber);
                Names.Add(singleNode.Data.PosNumber);
            }

            if (isVariablePosNumber)
            {
                if (!Names.Contains(singleNode.Data.Name) || !DrawingNumbers.Contains(singleNode.Data.DrawingNumber))
                {
                    Nodes.Add(singleNode);
                    Names.Add(singleNode.Data.Name);
                    DrawingNumbers.Add(singleNode.Data.DrawingNumber);
                }
            }

            _count++;

            Products cInstances = oInProduct.Products;

            SingleNode singleNodeInstance = null;

            if (_isActive)
            {
                for (int i = 1; i <= cInstances.Count; i++)
                {
                    Product oInst = cInstances.Item(i);
                    try
                    {
                        singleNodeInstance = new((Document)oInst.ReferenceProduct.Parent);
                    }
                    catch
                    {
                        _logger.Info($"Unable to retrieve the activity status of Product {oInst.get_Name()}");
                    }

                    _isAdapter = NodeValidator.IsAdapter(singleNodeInstance);

                    if (_isAdapter)
                    {
                        continue;
                    }

                    oInst.ApplyWorkMode(CatWorkModeType.DESIGN_MODE);
                    WalkDownTree(oInst, currentDepth + 1, maxDepthStr);
                }
            }
        }
    }
}
