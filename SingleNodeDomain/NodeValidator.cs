using INFITF;
using KnowledgewareTypeLib;
using NLog;
using ProductStructureTypeLib;

namespace TechBOM.SingleNodeDomain
{
    public class NodeValidator
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static bool IsRoot(Document document)
        {
            if (document == null)
            {
                _logger.Error("Document is null.");
                return false;
            }

            return document.Application.ActiveDocument.FullName == document.FullName;
        }

        public static bool IsAdapter(SingleNode node)
        {
            try
            {
                if (node.Data.UserRefProps.Count != 0)
                {
                    if (node.Data.PartNumber.Contains("_______V00_ADAPTER"))
                    {
                        return true;
                    }
                    else if (node.Data.PartNumber.Contains("BEZ_AXIS"))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch 
            {
                _logger.Error($"Error while checking if the node is an adapter - {node.Data.PartNumber}");
                return false;
            }
            
        }

        public static bool IsUb(SingleNode node) 
        {
            try
            {
                Parameter quelle = node.Data.Params.Item("Quelle");

                var partNumber = node.Data.PartNumber;

                string quelleValue = quelle.ValueAsString();

                if (quelleValue == "UB_Baugruppe")
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
                NodeExtender nodeExtender = new();

                nodeExtender.AddQuelleParameter(node);

                return false;
            }
        }

        public static bool HasUserRefProperties(SingleNode node)
        {
            return node.Data.UserRefProps.Count != 0;
        }


        public static bool IsCorrectStartModel(SingleNode node)
        {
            try
            {
                foreach (Parameter param in node.Data.UserRefProps)
                {
                    string paramName = param.get_Name();

                    if (paramName.Contains("ZNR"))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch 
            {
                _logger.Error($"Error while checking if the node is a correct start model - {node.Data.PartNumber}");

                return false;
            }
            
        }

        public static bool IsComponent(object docListItem)
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
                _logger.Error("Error while checking if the node is a component.");
                return false;
            }
        }

        public static bool IsActive(Product instance)
        {
            string parentPartNumber;
            string instanceName = string.Empty;
            try
            {
                parentPartNumber = ((Product)instance.Parent).get_PartNumber();
                instanceName = instance.get_Name();

                //crashed wenn instance is root product
                return instance.Parameters.Item(parentPartNumber + "\\" + instanceName + "\\Component Activation State").ValueAsString() == "true";
            }
            catch
            {
                _logger.Error($"Error while checking if the node is active - {instanceName}");
                //MessageBox.Show($"Es ist ein Problem in dem Knoten: \"{parentPartNumber}\" mit dem Part: \"{instanceName}\" aufgetreten");
                return false;
            }
        }
    }
}
