namespace Common.Enums;

public enum OrderStatus
{
    Pending,
    Preparing,
    ReadyForDelivery,
    OutForDelivery,
    Delivered,
    Canceled,
    Rejected
}