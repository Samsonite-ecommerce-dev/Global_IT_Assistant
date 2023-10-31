using System;

namespace Samsonite.Library.Web.Business.Custom
{
    public class KoreaApiToolRequest
    {
        #region Warehouse
        public class PostReplyRequest
        {
            /// <summary>
            /// 订单号
            /// </summary>
            public string OrderNo { get; set; }

            /// <summary>
            /// 子订单号
            /// </summary>
            public string SubOrderId { get; set; }

            /// <summary>
            /// 操作记录的类型
            /// </summary>
            public int Type { get; set; }

            /// <summary>
            /// 操作状态 1:成功,0:失败
            /// </summary>
            public int ReplyState { get; set; }

            /// <summary>
            /// 操作时间
            /// </summary>
            public string ReplyDate { get; set; }

            /// <summary>
            /// 回复内容
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 操作记录的ID
            /// </summary>
            public long Id { get; set; }
        }

        public class PostDeliveryRequest
        {
            /// <summary>
            /// 店铺SAP Code
            /// </summary>
            public string MallId { get; set; }

            /// <summary>
            /// 订单号
            /// </summary>
            public string OrderNo { get; set; }

            /// <summary>
            /// 子订单号
            /// </summary>
            public string SubOrderId { get; set; }

            /// <summary>
            /// 快递单号
            /// </summary>
            public string DeliveryNo { get; set; }

            /// <summary>
            /// 快递费用
            /// </summary>
            public string ReceiveCost { get; set; }

            /// <summary>
            /// 快递类型
            /// </summary>
            public string Type { get; set; }

            /// <summary> 
            /// 物流公司
            /// </summary>
            public string Company { get; set; }

            /// <summary>
            ///快递公司编码
            /// </summary>
            public string DeliveryCode { get; set; }

            /// <summary>
            /// SKU
            /// </summary>
            public string Sku { get; set; }

            /// <summary>
            /// 快递发货时间
            /// </summary>
            public string ReceiveDate { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public int Quantity { get; set; }

            /// <summary>
            /// 消息
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 发货人
            /// </summary>
            public string Receive { get; set; }

            /// <summary>
            /// 收件地址
            /// </summary>
            public string Address { get; set; }

            /// <summary>
            /// 收件人邮编
            /// </summary>
            public string Zipcode { get; set; }

            /// <summary>
            /// 发货仓库
            /// </summary>
            public string Warehouse { get; set; }
        }

        public class PostInventoryRequest
        {
            /// <summary>
            /// 产品Sku
            /// </summary>
            public string Sku { get; set; }

            /// <summary>
            /// 产品id
            /// </summary>
            public string ProductId { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public int Quantity { get; set; }
        }
        #endregion
    }
}
