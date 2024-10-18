using NPOI.SS.UserModel;

namespace TechBOM
{
    public class BomEntry(ISheet workSheet)
    {
        private ISheet _workSheet = workSheet;

        public void FillBomValues(SingleNode singleNode, int rowNumber)
        {
            List<string> parts = [];

            string finalRemarkValue;
            string remark = singleNode.Remark;
            string additionalInfo = singleNode.AdditionalInfo;
            string dinIso = singleNode.DinIso;

            if (singleNode.IsZsb) return;

            IRow currentRow = _workSheet.GetRow(13 + rowNumber);

            ICell oCellPosNumber = currentRow.GetCell(0);
            oCellPosNumber.SetCellValue(singleNode.PosNumber);

            ICell oCellRevision = currentRow.GetCell(1);
            oCellRevision.SetCellValue(singleNode.Revision);

            ICell oCellQuantity = currentRow.GetCell(2);
            oCellQuantity.SetCellValue(singleNode.Quantity);

            ICell oCellName = currentRow.GetCell(4);
            oCellName.SetCellValue(singleNode.Name);

            ICell oCellOrderNumber = currentRow.GetCell(6);
            oCellOrderNumber.SetCellValue(singleNode.DrawingNumber);

            if (string.IsNullOrWhiteSpace(singleNode.DrawingNumber))
            {
                oCellOrderNumber.SetCellValue(singleNode.TypeDescription);
            }

            ICell oCellItemNumber = currentRow.GetCell(8);
            oCellItemNumber.SetCellValue(singleNode.ItemNumber);

            ICell oCellMaterial = currentRow.GetCell(10);
            oCellMaterial.SetCellValue(singleNode.MaterialNumber);

            ICell oCelldimensions = currentRow.GetCell(11);
            oCelldimensions.SetCellValue(singleNode.Dimensions);

            ICell oCellSupplier = currentRow.GetCell(5);
            oCellSupplier.SetCellValue(singleNode.Manufacturerer);

            ICell oCellSapNumber = currentRow.GetCell(9);
            oCellSapNumber.SetCellValue(singleNode.SapNumber);

            ICell oCellSparePart = currentRow.GetCell(15);
            oCellSparePart.SetCellValue(singleNode.SparePart);

            ICell oCellInfo = currentRow.GetCell(16);

            if (!string.IsNullOrWhiteSpace(remark))
            {
                parts.Add(remark);
            }

            if (!string.IsNullOrWhiteSpace(additionalInfo))
            {
                parts.Add(additionalInfo);
            }

            if (!string.IsNullOrWhiteSpace(dinIso))
            {
                parts.Add(dinIso);
            }

            finalRemarkValue = string.Join(";", parts);

            oCellInfo.SetCellValue(finalRemarkValue);
        }
    }
}
