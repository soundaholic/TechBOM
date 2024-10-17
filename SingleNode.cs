using NLog;
using INFITF;
using KnowledgewareTypeLib;
using MECMOD;
using ProductStructureTypeLib;
using System;

namespace TechBOM
{
    public class SingleNode
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public string PartNumber { get; set; }
        public Parameters UserRefProps { get; set; }
        public string PosNumber { get; set; }
        public string Revision { get; set; }
        public string Quantity { get; set; }
        public string Name { get; set; }
        public string DrawingNumber { get; set; }
        public string ItemNumber { get; set; }
        public string TypeDescription { get; set; }
        public string DinIso { get; set; }
        public string MaterialNumber { get; set; }
        public string Dimensions { get; set; }
        public string Manufacturerer { get; set; }
        public string SapNumber { get; set; }
        public string   AdditionalInfo { get; set; }
        public string Remark { get; set; }
        public string SparePart { get; set; }
        public bool IsZsb { get; }

        private readonly Document _oDocument;
        private Product _oProduct;
        private Part? _oPart;

        public SingleNode(Document oDocument)
        {
            _oDocument = oDocument;
            IsZsb = _oDocument.Application.ActiveDocument.FullName.Equals(_oDocument.FullName);
            GetValuesParameters();
        }

        public void GetValuesParameters()
        {
            //if (_oDocument == null)
            //{
            //    _logger.Error("Document is null");
            //    return;
            //}
            //if (_oProduct == null)
            //{
            //    _logger.Error("Product is null");
            //    return;
            //}

            int count = 0;

            if (_oDocument is PartDocument oPartDocument)
            {
                _oPart = oPartDocument.Part;
                _oProduct = oPartDocument.Product;
                PartNumber = _oPart.get_Name();
                if (Counter.Instance.PartCount.TryGetValue(PartNumber, out int value))
                {
                    count = value;
                }
                else
                {
                    count = 0;
                }

            }
            else if (_oDocument is ProductDocument oProductDocument)
            {
                _oProduct = oProductDocument.Product;
                PartNumber = _oProduct.get_Name();
                count = IsZsb ? 1 : Counter.Instance.PartCount[PartNumber];
            }

            UserRefProps = _oProduct.UserRefProperties;
            _logger.Trace("");

            if (UserRefProps.Count != 0)
            {
                try
                {
                    Name = UserRefProps.Item("BENENNUNG").ValueAsString();

                    DrawingNumber = UserRefProps.Item("ZNR").ValueAsString();

                    PosNumber = UserRefProps.Item("POS_NR").ValueAsString();
                    _logger.Trace($"Positionsnummer: {PosNumber}");

                    Revision = UserRefProps.Item("ZI").ValueAsString();
                    _logger.Trace($"Revision: {Revision}");

                    Quantity = count.ToString();
                    _logger.Trace($"Quantity: {Quantity}");

                    ItemNumber = UserRefProps.Item("ARTIKEL_NR").ValueAsString();
                    _logger.Trace($"ItemNumber: {ItemNumber}");

                    TypeDescription = UserRefProps.Item("TYPENBEZ").ValueAsString();
                    _logger.Trace($"ItemNumber: {TypeDescription}");

                    DinIso = UserRefProps.Item("DIN_EN_ISO").ValueAsString();
                    _logger.Trace($"DinIso: {DinIso}");

                    MaterialNumber = UserRefProps.Item("MAT_NR").ValueAsString();
                    _logger.Trace($"MaterialNumber: {MaterialNumber}");

                    try
                    {
                        Dimensions = UserRefProps.Item("ZUSCHNITT").ValueAsString();
                        _logger.Trace($"Dimensions: {Dimensions}");
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, $"Das Teil mit PosNr: {PosNumber} hat keine ZUSCHNITT Parameter");
                    }

                    Manufacturerer = UserRefProps.Item("LIEFERANT").ValueAsString();
                    _logger.Trace($"Manufacturerer: {Manufacturerer}");

                    SapNumber = UserRefProps.Item("SAP_IDENTNR").ValueAsString();
                    _logger.Trace($"SapNumber: {SapNumber}");

                    AdditionalInfo = UserRefProps.Item("ZUSATZINFO").ValueAsString();
                    _logger.Trace($"AdditionalInfo: {AdditionalInfo}");

                    Remark = UserRefProps.Item("BEMERKUNG").ValueAsString();
                    _logger.Trace($"Remark: {Remark}");

                    SparePart = UserRefProps.Item("ERSATZ").ValueAsString();
                    _logger.Trace($"SparePart: {SparePart}");
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
