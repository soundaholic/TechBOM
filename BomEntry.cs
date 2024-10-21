using NPOI.SS.UserModel;
using TechBOM.SingleNodeDomain;

namespace TechBOM
{
    public class BomEntry(ISheet workSheet)
    {
        private ISheet _workSheet = workSheet;

        public void FillBomValues(SingleNode singleNode, int rowNumber)
        {
            List<string> parts = [];

            string finalRemarkValue;
            string remark = singleNode.Data.Remark;
            string additionalInfo = singleNode.Data.AdditionalInfo;
            string dinIso = singleNode.Data.DinIso;

            if (singleNode.IsZsb) return;

            IRow currentRow = _workSheet.GetRow(13 + rowNumber);

            ICell oCellPosNumber = currentRow.GetCell(0);
            oCellPosNumber.SetCellValue(singleNode.Data.PosNumber);

            ICell oCellRevision = currentRow.GetCell(1);
            oCellRevision.SetCellValue(singleNode.Data.Revision);

            ICell oCellQuantity = currentRow.GetCell(2);
            oCellQuantity.SetCellValue(singleNode.Data.Quantity);

            ICell oCellName = currentRow.GetCell(4);
            oCellName.SetCellValue(singleNode.Data.Name);

            ICell oCellOrderNumber = currentRow.GetCell(6);
            oCellOrderNumber.SetCellValue(singleNode.Data.DrawingNumber);

            if (string.IsNullOrWhiteSpace(singleNode.Data.DrawingNumber))
            {
                oCellOrderNumber.SetCellValue(singleNode.Data.TypeDescription);
            }

            ICell oCellItemNumber = currentRow.GetCell(8);
            oCellItemNumber.SetCellValue(singleNode.Data.ItemNumber);

            ICell oCellMaterial = currentRow.GetCell(10);
            oCellMaterial.SetCellValue(singleNode.Data.MaterialNumber);

            ICell oCelldimensions = currentRow.GetCell(11);
            oCelldimensions.SetCellValue(singleNode.Data.Dimensions);

            ICell oCellSupplier = currentRow.GetCell(5);
            oCellSupplier.SetCellValue(singleNode.Data.Manufacturerer);

            ICell oCellSapNumber = currentRow.GetCell(9);
            oCellSapNumber.SetCellValue(singleNode.Data.SapNumber);

            ICell oCellSparePart = currentRow.GetCell(15);
            oCellSparePart.SetCellValue(singleNode.Data.SparePart);

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
