using NLog;
using INFITF;
using KnowledgewareTypeLib;
using MECMOD;
using ProductStructureTypeLib;

namespace TechBOM
{
    public class SingleNode
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public string PartNumber { get; set; } = string.Empty;
        public Parameters UserRefProps { get; set; } = default!;
        public string PosNumber { get; set; } = string.Empty;
        public string Revision { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string DrawingNumber { get; set; } = string.Empty;
        public string ItemNumber { get; set; } = string.Empty;
        public string TypeDescription { get; set; } = string.Empty;
        public string DinIso { get; set; } = string.Empty;
        public string MaterialNumber { get; set; } = string.Empty;
        public string Dimensions { get; set; } = string.Empty;
        public string Manufacturerer { get; set; } = string.Empty;
        public string SapNumber { get; set; } = string.Empty;
        public string AdditionalInfo { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public string SparePart { get; set; } = string.Empty;
        public bool IsZsb { get; }

        private readonly Document _oDocument;
        private Product _oProduct = default!;
        private Part? _oPart;

        public SingleNode(Document oDocument)
        {
            _oDocument = oDocument;
            IsZsb = _oDocument.Application.ActiveDocument.FullName.Equals(_oDocument.FullName);
            
            if (oDocument is PartDocument oPartDocument)
            {
                _oPart = oPartDocument.Part;
                _oProduct = oPartDocument.Product;
                PartNumber = _oProduct.get_PartNumber();
            }
            else if (oDocument is ProductDocument oProductDocument)
            {
                _oProduct = oProductDocument.Product;
                PartNumber = _oProduct.get_PartNumber();
            }
            GetValuesParameters();
        }

        public void GetValuesParameters()
        {
            if (_oDocument is PartDocument oPartDocument)
            {
                _oPart = oPartDocument.Part;
                _oProduct = oPartDocument.Product;

            }
            else if (_oDocument is ProductDocument oProductDocument)
            {
                _oProduct = oProductDocument.Product;
            }

            UserRefProps = _oProduct.UserRefProperties;

            if (UserRefProps.Count != 0)
            {
                try
                {
                    Name = UserRefProps.Item("BENENNUNG").ValueAsString();
                    DrawingNumber = UserRefProps.Item("ZNR").ValueAsString();
                    PosNumber = UserRefProps.Item("POS_NR").ValueAsString();
                    Revision = UserRefProps.Item("ZI").ValueAsString();
                    Quantity = Counter.Instance.GetCount(PartNumber).ToString();
                    ItemNumber = UserRefProps.Item("ARTIKEL_NR").ValueAsString();
                    TypeDescription = UserRefProps.Item("TYPENBEZ").ValueAsString();
                    DinIso = UserRefProps.Item("DIN_EN_ISO").ValueAsString();
                    MaterialNumber = UserRefProps.Item("MAT_NR").ValueAsString();

                    try
                    {
                        Dimensions = UserRefProps.Item("ZUSCHNITT").ValueAsString();
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, $"Das Teil mit PosNr: {PosNumber} hat keine ZUSCHNITT Parameter");
                    }

                    Manufacturerer = UserRefProps.Item("LIEFERANT").ValueAsString();
                    SapNumber = UserRefProps.Item("SAP_IDENTNR").ValueAsString();
                    AdditionalInfo = UserRefProps.Item("ZUSATZINFO").ValueAsString();
                    Remark = UserRefProps.Item("BEMERKUNG").ValueAsString();
                    SparePart = UserRefProps.Item("ERSATZ").ValueAsString();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"User Properties were not found: {e.Message}");
                }
            }
            else
            {
                _logger.Error("UserRefProperties is empty");
            }
        }
    }
}
