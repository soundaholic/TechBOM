using INFITF;
using KnowledgewareTypeLib;
using ProductStructureTypeLib;

namespace TechBOM.SingleNodeDomain
{
    public class NodeValidator
    {
        public static bool IsRoot(Document document)
        {
            string name1 = document.Application.ActiveDocument.FullName;
            string name2 = document.FullName;

            return name1 == name2;
        }

        public static bool IsAdapter(SingleNode node)
        {
            if (node.Data.UserRefProps.Count != 0)
            {
                //if (!string.IsNullOrWhiteSpace(node.Data.Name))
                //{
                //    return node.Data.Name.ToLower() == "adapter".ToLower();
                //}
                if (node.Data.PartNumber.Contains("V00_ADAPTER")) 
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
            foreach (KnowledgewareTypeLib.Parameter param in node.Data.UserRefProps)
            {
                string paramName = param.get_Name();

                if (paramName.Contains("ZNR"))
                {
                    return true;
                }
            }
            return false;
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
                return false;
            }
        }

        public static bool IsActive(Product instance)
        {
            string parentPartNumber = string.Empty;
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
                MessageBox.Show($"Es ist ein Problem in dem Knoten: \"{parentPartNumber}\" mit dem Part: \"{instanceName}\" aufgetreten");
                return false;
            }
        }
    }
}
