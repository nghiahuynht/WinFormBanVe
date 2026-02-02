public class ReportGridModel
{
    public Int64 id { get; set; }
    public string roomName { get; set; }
    public decimal? totalBill { get; set; }
    public string createdBy { get; set; }
    public DateTime? createdDate { get; set; }

    // ✅ Thêm các trường khách hàng
    public string cusPhone { get; set; }
    public string cusName { get; set; }
    public decimal? cusDiscountPercent { get; set; }
    public decimal? cusTotalBeforeDiscount { get; set; }
    public decimal? cusAmountDiscount { get; set; }
    public decimal? cusTotalMoneyAfterCusDiscount { get; set; }
    public string statusName { get; set; }
}