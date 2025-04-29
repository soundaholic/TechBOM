using KnowledgewareTypeLib;
using TechBOM.Forms;

namespace TechBOM.SingleNodeDomain
{
    public class NodeExtender
    {
        public void AddQuelleParameter(SingleNode node) 
        {
            object[] parameters = { "ZSB_Baugruppe", "UB_Baugruppe", "Fertigungsteil", "Normteil", "Kaufteil", "HB_Normteil_Kaufteil_Standard" };

            try
            {
                if (node.Data.PartNumber != "")
                {
                    using (var form = new QuelleSelection(parameters))
                    {
                        form.label_Name.Text = node.Data.PartNumber + " hat keinen Parameter \"Quelle\"";

                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            string selectedParameter = form.SelectedValue;

                            StrParam xx = node.Data.Params.CreateString("Quelle", "");
                            xx.SetEnumerateValues(parameters);
                            xx.ValuateFromString(selectedParameter);
                        }
                        else if (form.ShowDialog() == DialogResult.Cancel)
                        {
                            //return false;
                        }
                    }
                }
            }
            catch
            {
                //return false;
            }

        }
    }
}
