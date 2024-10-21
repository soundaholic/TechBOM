using INFITF;
using ProductStructureTypeLib;

namespace TechBOM.SingleNodeDomain
{
    public class NodeValidator
    {
        public bool IsZsb(Document document)
        {
            return document.Application.ActiveDocument.FullName.Equals(document.FullName);
        }

        public bool IsAdapter(SingleNode node)
        {
            return node.Data.Name.ToLower() == "adapter".ToLower();
        }

        public bool IsComponent(object docListItem)
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

        public bool IsActive(Product instance)
        {
            //crashed wenn instance is root product
            return instance.Parameters.Item(((Product)instance.Parent).get_PartNumber() + "\\" + instance.get_Name() + "\\Component Activation State").ValueAsString() == "true";
        }
    }
}
