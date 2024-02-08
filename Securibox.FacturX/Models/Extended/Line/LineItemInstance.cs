namespace Securibox.FacturX.Models.Extended
{
    public class LineItemInstance
    {
        public string? BatchId { get; private set; }
        public string? SupplierSerialId { get; private set; }

        public LineItemInstance(string? batchId, string? supplierSerialId)
        {
            BatchId = batchId;
            SupplierSerialId = supplierSerialId;
        }
    }
}
