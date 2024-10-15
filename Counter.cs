using ProductStructureTypeLib;

namespace TechBOM
{
    public class Counter
    {
        private static Counter _instance;

        public Dictionary<string, int> PartCount { get; private set; }

        private Counter()
        {
            PartCount = new Dictionary<string, int>();
        }

        public static Counter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Counter();
                }
                return _instance;
            }
        }

        // Метод для сброса словаря PartCount
        public void Reset()
        {
            PartCount.Clear();
        }

        public void CountParts(Product product, int currentDepth, string maxDepthStr)
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

            CatiaHelper.Instance.CatHelperReset();

            Products products = product.Products;

            for (int i = 1; i <= products.Count; i++)
            {
                Product subProduct = products.Item(i);

                bool isActive = CatiaHelper.Instance.IsProductActivated(subProduct);

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

                    // Recursive call to count parts in subprojects
                    if (subProduct.Products.Count > 0)
                    {
                        CountParts(subProduct, currentDepth + 1, maxDepthStr);
                    }
                }
            }
        }
    }
}
