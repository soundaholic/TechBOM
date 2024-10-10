using INFITF;
using NLog;
using ProductStructureTypeLib;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TechBOM
{
    public class CatiaHelper
    {
        public List<string> Names { get; set; } = new List<string>();
        public List<SingleNode> Nodes { get; set; } = new List<SingleNode>();

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        //private string _currentNodeName;

        private bool _isNameUnique;
        private bool _isNotZsb;
        private bool _isNotUBPart;
        private bool _isVariablePosNumber;
        private bool _isAdapter;
        private bool _isNotComponent;
        private bool _isAspPart;


        public void CatHelperReset()
        {
            Names.Clear();
            Nodes.Clear();
        }

        public void WalkDownTree(Product oInProduct, int currentDepth, string maxDepthStr)
        {
            // Если глубина "All", то отключаем проверку максимальной глубины
            bool unlimitedDepth = maxDepthStr == "All";

            // Если не "All", то пытаемся преобразовать строку в число для ограничения глубины
            if (!unlimitedDepth)
            {
                if (!int.TryParse(maxDepthStr, out int maxDepth))
                {
                    throw new ArgumentException("Invalid depth value. It must be a number or 'All'.");
                }

                maxDepth += 1;

                // Если текущая глубина рекурсии превышает максимальную, прекращаем выполнение
                if (currentDepth >= maxDepth)
                {
                    return;
                }
            }

            Products cInstances = oInProduct.Products;
            
            bool isActive = true;
            string posNumberFromPartname = string.Empty;

            Document oDoc = (Document)oInProduct.ReferenceProduct.Parent;

            if (!IsComponent(oInProduct))
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

            SingleNode singleNode = new SingleNode(oDoc);

            try
            {
                isActive = IsProductActivated(oInProduct);
            }
            catch
            {
            }

            bool isNameUnique = !Names.Contains(singleNode.DrawingNumber) || !Names.Contains(singleNode.PosNumber);
            bool isNotZsb = !singleNode.IsZsb;
            bool isNotUBPart = !posNumberFromPartname.Contains("UB");
            bool isVariablePosNumber = singleNode.PosNumber == "XXXXX";

            bool isNotComponent = !IsComponent(oInProduct);
            bool isAspPart = singleNode.PosNumber == "00000";

            bool canAddNode = isNameUnique && isNotZsb && isNotUBPart && isActive && !isVariablePosNumber && !_isAdapter && isNotComponent && !isAspPart;

            if (canAddNode)
            {
                Nodes.Add(singleNode);
                Names.Add(singleNode.DrawingNumber);
                Names.Add(singleNode.PosNumber);
            }

            // Добавление узла, если его нет в списке "Nodes"
            if (isVariablePosNumber)
            {
                if (!Names.Contains(singleNode.Name))
                {
                    Nodes.Add(singleNode);
                    Names.Add(singleNode.Name);
                }
            }

            for (int i = 1; i <= cInstances.Count; i++)
            {
                Product oInst = cInstances.Item(i);
                SingleNode singleNodeInstance = new SingleNode((Document)oInst.ReferenceProduct.Parent);
                _isAdapter = singleNodeInstance.Name == "ADAPTER";

                if (_isAdapter)
                {
                    continue;
                }

                oInst.ApplyWorkMode(CatWorkModeType.DESIGN_MODE);

                // Рекурсивный вызов с увеличением текущей глубины
                WalkDownTree(oInst, currentDepth + 1, maxDepthStr);
            }
        }



        public bool IsProductActivated(Product instance)
        {
            //crashed wenn instance is root product
            return instance.Parameters.Item(((Product)instance.Parent).get_PartNumber() + "\\" + instance.get_Name() + "\\Component Activation State").ValueAsString() == "true";
        }

        private bool IsComponent(object docListItem)
        {
            try
            {
                Product product = (Product)docListItem;
                Reference docRef = (Reference)docListItem;
                ProductDocument productDoc = (ProductDocument)product.ReferenceProduct.Parent;

                if (product.get_PartNumber() != productDoc.Product.get_PartNumber())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
