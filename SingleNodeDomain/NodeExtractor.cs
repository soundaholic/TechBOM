using INFITF;
using KnowledgewareTypeLib;
using MECMOD;
using NLog;
using ProductStructureTypeLib;

namespace TechBOM.SingleNodeDomain
{
    public class NodeExtractor
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public NodeData Extract(Document document)
        {
            var nodeData = new NodeData();
            try
            {
                nodeData.PartNumber = GetPartNumber(document);

                var userRefProps = GetUserRefProperties(document);
                nodeData.Name = GetProperty(userRefProps, "BENENNUNG");
                nodeData.DrawingNumber = GetProperty(userRefProps, "ZNR");
                nodeData.PosNumber = GetProperty(userRefProps, "POS_NR");
                nodeData.Revision = GetProperty(userRefProps, "ZI");
                nodeData.Quantity = Counter.Instance.GetCount(nodeData.PartNumber).ToString();
                nodeData.ItemNumber = GetProperty(userRefProps, "ARTIKEL_NR");
                nodeData.TypeDescription = GetProperty(userRefProps, "TYPENBEZ");
                nodeData.DinIso = GetProperty(userRefProps, "DIN_EN_ISO");
                nodeData.MaterialNumber = GetProperty(userRefProps, "MAT_NR");
                nodeData.Dimensions = TryGetProperty(userRefProps, "ZUSCHNITT");
                nodeData.Manufacturerer = GetProperty(userRefProps, "LIEFERANT");
                nodeData.SapNumber = GetProperty(userRefProps, "SAP_IDENTNR");
                nodeData.AdditionalInfo = GetProperty(userRefProps, "ZUSATZINFO");
                nodeData.Remark = GetProperty(userRefProps, "BEMERKUNG");
                nodeData.SparePart = GetProperty(userRefProps, "ERSATZ");
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error while extracting node properties.");
            }

            return nodeData;
        }

        private string GetProperty(Parameters userRefProps, string propName)
        {
            return userRefProps.Item(propName).ValueAsString();
        }

        private string TryGetProperty(Parameters userRefProps, string propName)
        {
            try
            {
                return userRefProps.Item(propName).ValueAsString();
            }
            catch
            {
                _logger.Warn($"Property {propName} not found.");
                return string.Empty;
            }
        }

        private Parameters GetUserRefProperties(Document document)
        {
            if (document is PartDocument partDoc)
            {
                return partDoc.Product.UserRefProperties;
            }
            else if (document is ProductDocument productDoc)
            {
                return productDoc.Product.UserRefProperties;
            }
            throw new ArgumentException("Unsupported document type.");
        }

        private string GetPartNumber(Document document)
        {
            try
            {
                string partNumber = string.Empty;
                Product? oProduct = null;

                if (document is PartDocument oPartDocument)
                {
                    oProduct = oPartDocument.Product;
                    partNumber = oProduct.get_PartNumber();
                }
                else if (document is ProductDocument oProductDocument)
                {
                    oProduct = oProductDocument.Product;
                    partNumber = oProduct.get_PartNumber();
                }
                return partNumber;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Unsupported document type.", ex);
            }
        }
    }

}
