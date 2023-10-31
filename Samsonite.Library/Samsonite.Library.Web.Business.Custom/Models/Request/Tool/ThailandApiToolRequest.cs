using System;

namespace Samsonite.Library.Web.Business.Custom
{
    public class ThailandApiToolRequest
    {
        #region Warehouse
        public class PostReplyRequest
        {
            /// <summary>
            /// 店铺sapcode
            /// </summary>
            public string MallCode { get; set; }

            /// <summary>
            /// 订单号
            /// </summary>
            public string OrderNo { get; set; }

            /// <summary>
            /// 子订单号
            /// </summary>
            public string SubOrderNo { get; set; }

            /// <summary>
            /// 操作记录的类型
            /// </summary>
            public int Type { get; set; }

            /// <summary>
            /// 操作时间
            /// </summary>
            public string ReplyDate { get; set; }

            /// <summary>
            /// 操作状态 
            /// 1.type=0和9时:1.成功,2.失败
            /// 2.type=其它时:1.成功,2:失败,3:处理中
            /// </summary>
            public int ReplyState { get; set; }

            /// <summary>
            /// 回复内容
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 操作记录的ID
            /// </summary>
            public long RecordId { get; set; }
        }

        public class PostDeliveryRequest
        {
            /// <summary>
            /// 店铺sapcode
            /// </summary>
            public string MallCode { get; set; }

            /// <summary>
            /// 订单号
            /// </summary>
            public string OrderNo { get; set; }

            /// <summary>
            /// 子订单号
            /// </summary>
            public string SubOrderNo { get; set; }

            /// <summary> 
            /// sku
            /// </summary>
            public string Sku { get; set; }

            /// <summary>
            ///快递公司编码
            /// </summary>
            public string DeliveryCode { get; set; }

            /// <summary> 
            /// 快递公司
            /// </summary>
            public string Company { get; set; }

            /// <summary>
            /// 快递单号(多个快递号用逗号隔开)
            /// </summary>
            public string DeliveryNo { get; set; }

            /// <summary>
            /// 包裹数量
            /// </summary>
            public int Packages { get; set; }

            /// <summary>
            /// 快递类型
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// 快递费用
            /// </summary>
            public decimal ReceiveCost { get; set; }

            /// <summary>
            /// 发货仓库
            /// </summary>
            public string Warehouse { get; set; }

            /// <summary>
            /// 接单时间
            /// </summary>
            public string ReceiveDate { get; set; }

            /// <summary>
            /// 理货时间
            /// </summary>
            public string DealDate { get; set; }

            /// <summary>
            /// 快递发货时间
            /// </summary>
            public string SendDate { get; set; }
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
            /// 产品类型(1普通产品2套装3赠品)
            /// </summary>
            public int ProductType { get; set; }

            /// <summary>
            /// 数量
            /// </summary>
            public int Quantity { get; set; }
        }

        public class PostDetailRequest
        {
            /// <summary>
            /// 店铺sapCode
            /// </summary>
            public string MallCode { get; set; }

            /// <summary>
            /// 订单号
            /// </summary>
            public string OrderNo { get; set; }

            /// <summary>
            /// 子订单号
            /// </summary>
            public string SubOrderNo { get; set; }

            /// <summary>
            /// 类型
            /// </summary>
            public int Type { get; set; }

            /// <summary>
            /// 类型对应详情
            /// </summary>
            public object Data { get; set; }
        }

        /// <summary>
        /// 物流状态
        /// </summary>
        public class PostShippingStatusResponse
        {
            /// <summary>
            /// 状态
            /// </summary>
            public string Status { get; set; }
        }

        /// <summary>
        /// 物流详情
        /// </summary>
        public class PostExpressDetailResponse
        {
            /// <summary>
            /// 状态
            /// </summary>
            public int Status { get; set; }
            /// <summary>
            /// 详情
            /// </summary>
            public string Detail { get; set; }
        }
        #endregion
    }
}
