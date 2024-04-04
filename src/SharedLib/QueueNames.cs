namespace SharedLib;
public class QueueNames
{
    public const string OrderSaga = "order-saga-queue";
    public const string StockOrderCreated = "stock-order-created-queue";
    public const string StockReserved = "stock-reserved-queue";
    public const string OrderStockNotReserved = "order-stock-not-reserved-queue";
    public const string StockRollback = "stock-rollback-queue";
    public const string StockPaymentFailed = "stock-payment-failed-queue";
    public const string OrderPaymentFailed = "order-payment-failed-queue";
    public const string PaymentStockReservedRequest = "payment-stock-reserved-request-queue";
    public const string OrderRequestCompleted = "order-request-completed-queue";
    public const string OrderRequestFailed = "order-request-failed-queue";
}
