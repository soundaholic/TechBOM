using INFITF;
using ProductStructureTypeLib;
using System.IO;
using Application = INFITF.Application;

namespace TechBOM
{
    public class RootNode
    {
        public Document ActiveDoc { get; set; } 

        private static Application _catia = CatiaConnect.Instance.ConnectCatia();

        public string SaveFileName { get; set; }
        public string SavePath { get; private set; } = string.Empty;
        public string RootPartNumber { get; private set; } = string.Empty;
        public string NameForTextBox { get; private set; } = string.Empty;
        public string DrawingNumberForTextBox { get; private set; } = string.Empty;
        public string VersionForTextBox { get; private set; } = string.Empty;


        public RootNode()
        {
            ActiveDoc = _catia.ActiveDocument;
            GetHeadParameters();
            SaveFileName = Path.Combine(SavePath, $"{RootPartNumber}.xlsx");
        }

        private void GetHeadParameters()
        {
            if (ActiveDoc is ProductDocument oProductDoc)
            {
                SavePath = oProductDoc.Path;
                RootPartNumber = oProductDoc.Product.get_Name();
            }

            SingleNode singleNode = new(ActiveDoc);

            NameForTextBox = singleNode.Name;
            DrawingNumberForTextBox = singleNode.DrawingNumber;
            VersionForTextBox = singleNode.Revision;
        }

    }
}
