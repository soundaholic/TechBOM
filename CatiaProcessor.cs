using INFITF;
using NLog;
using ProductStructureTypeLib;
using TechBOM.SingleNodeDomain;

namespace TechBOM
{
    public class CatiaProcessor
    {
        public List<string> Names { get; set; } = [];
        public List<SingleNode> Nodes { get; set; } = [];
        
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        
        private bool _isAdapter;

        private static CatiaProcessor _instance = new();

        public static CatiaProcessor Instance
        {
            get
            {
                _instance ??= new CatiaProcessor();
                return _instance;
            }
        }

        public void CatHelperReset()
        {
            Names.Clear();
            Nodes.Clear();
        }

        public void WalkDownTree(Product oInProduct, int currentDepth, string maxDepthStr)
        {
            NodeValidator nodeValidator = new();

            Document oDoc = (Document)oInProduct.ReferenceProduct.Parent;

            SingleNode singleNode = new(oDoc);

            bool isActive = true;
            bool isAdapter = nodeValidator.IsAdapter(singleNode);
            bool isNameUnique = !Names.Contains(singleNode.Data.DrawingNumber) || !Names.Contains(singleNode.Data.PosNumber);
            bool isNotZsb = !singleNode.IsZsb;
            bool isVariablePosNumber = singleNode.Data.PosNumber == "XXXXX";
            bool isNotComponent = !nodeValidator.IsComponent(oInProduct);
            bool isAspPart = singleNode.Data.PosNumber == "00000";
            
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

            Products cInstances = oInProduct.Products;
            
            string posNumberFromPartname = string.Empty;

            if (isNotComponent)
            {
                string partNumber = oInProduct.get_PartNumber();

                if (partNumber.Length >= 17)
                {
                    posNumberFromPartname = partNumber.Substring(12, 5);
                }
                else
                {
                    posNumberFromPartname = partNumber.Substring(0, 5);
                }
            }
            else
            {
                string partName = oInProduct.get_PartNumber();
            }

            bool isNotUBPart = !posNumberFromPartname.Contains("UB");
            bool canAddNode = isNameUnique && isNotZsb && isNotUBPart && isActive && !isVariablePosNumber && !isAdapter && isNotComponent && !isAspPart;

            try
            {
                string partNumber = oInProduct.get_PartNumber();
                isActive = nodeValidator.IsActive(oInProduct);

                if (isActive)
                {
                    _logger.Info($"Product {partNumber} is active.");
                }
                else
                {
                    _logger.Warn($"Product {partNumber} is not active.");
                }
            }
            catch 
            {
                _logger.Info($"Unable to retrieve the activity status of Product {singleNode.Data.Name}");
            }
            
            
            if (canAddNode)
            {
                Nodes.Add(singleNode);
                Names.Add(singleNode.Data.DrawingNumber);
                Names.Add(singleNode.Data.PosNumber);
            }

            if (isVariablePosNumber)
            {
                if (!Names.Contains(singleNode.Data.Name))
                {
                    Nodes.Add(singleNode);
                    Names.Add(singleNode.Data.Name);
                }
            }

            for (int i = 1; i <= cInstances.Count; i++)
            {
                Product oInst = cInstances.Item(i);
                SingleNode singleNodeInstance = new((Document)oInst.ReferenceProduct.Parent);

                isAdapter = nodeValidator.IsAdapter(singleNodeInstance);

                if (isAdapter)
                {
                    continue;
                }

                oInst.ApplyWorkMode(CatWorkModeType.DESIGN_MODE);
                WalkDownTree(oInst, currentDepth + 1, maxDepthStr);
            }
        }
    }
}
