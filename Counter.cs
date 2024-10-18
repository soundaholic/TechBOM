using ProductStructureTypeLib;

namespace TechBOM
{
    public class Counter
    {
        private static Counter _instance = new();

        public Dictionary<string, int> PartCount { get; private set; }

        private Counter()
        {
            PartCount = [];
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

            CatiaProcessor.Instance.CatHelperReset();

            Products products = oProduct.Products;

            for (int i = 1; i <= products.Count; i++)
            {
                Product subProduct = products.Item(i);

                bool isActive = CatiaProcessor.Instance.IsProductActivated(subProduct);

                string partNumber = subProduct.get_PartNumber();

                if (isActive)
                {
                    if (PartCount.ContainsKey(partNumber))
                    {
                        PartCount[partNumber]++;
                    }
                    else
                    {
                        PartCount[partNumber] = 1;
                    }

                    // Recursive call to count parts in subproducts
                    if (subProduct.Products.Count > 0)
                    {
                        CountParts(subProduct, currentDepth + 1, maxDepthStr);
                    }
                }
            }
        }
    }
}
