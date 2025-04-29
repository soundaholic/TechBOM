using NLog;
using ProductStructureTypeLib;
using System.Collections.Concurrent;
using TechBOM.Interfaces;
using TechBOM.SingleNodeDomain;

namespace TechBOM
{
    public class Counter : ICounter
    {
        Logger _logger = LogManager.GetCurrentClassLogger();

        private static Counter _instance = new();

        // Use ConcurrentDictionary to make it thread-safe
        public ConcurrentDictionary<string, int> PartCount { get; private set; }

        public static int CountOfCoruptedParts { get; private set; }

        //public Dictionary<string, int> PartCount { get; private set; }

        public Counter()
        {
            PartCount = new ConcurrentDictionary<string, int>();  // Initialize with a new Dictionary
        }

        public static Counter Instance
        {
            get
            {
                _instance ??= new Counter();
                return _instance;
            }
        }

        public void Reset()
        {
            PartCount.Clear();
        }

        public int GetCount(string partNumber)
        {
            if (PartCount.TryGetValue(partNumber, out int value))
            {
                return value;
            }
            else
            {
                return 0;
            }
        }

        public void CountParts(Product oProduct, int currentDepth, string maxDepthStr)
        {
            string partNumber = "";
            NodeValidator nodeValidator = new();

            // if the depth is "All", then disable the maximum depth check
            bool unlimitedDepth = maxDepthStr == "All";

            // if not "All", then try to convert the string to a number to limit the depth
            if (!unlimitedDepth)
            {
                if (!int.TryParse(maxDepthStr, out int maxDepth))
                {
                    throw new ArgumentException("Invalid depth value. It must be a number or 'All'.");
                }

                maxDepth += 1;

                // if the current recursion depth exceeds the maximum, stop execution
                if (currentDepth >= maxDepth)
                {
                    return;
                }
            }

            //CatiaProcessor.Instance.CatHelperReset();

            Products products = oProduct.Products;

            for (int i = 1; i <= products.Count; i++)
            {
                Product subProduct = products.Item(i);

                bool isActive = NodeValidator.IsActive(subProduct);

                try
                {
                    partNumber = subProduct.get_PartNumber();
                }
                catch
                {
                    CountOfCoruptedParts++;
                }
                

                if (isActive)
                {
                    // Use AddOrUpdate to handle thread-safe incrementing of parts
                    PartCount.AddOrUpdate(partNumber, 1, (key, oldValue) => oldValue + 1);

                    // Recursive call to count parts in subproducts
                    if (subProduct.Products.Count > 0)
                    {
                        CountParts(subProduct, currentDepth + 1, maxDepthStr);
                    }
                }
                else
                {
                    // If the product is not active, log a warning
                    _logger.Warn($"Product {partNumber} is not active.");
                }
            }
        }
    }
}
