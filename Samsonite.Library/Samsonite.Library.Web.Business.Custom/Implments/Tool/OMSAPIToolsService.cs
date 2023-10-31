using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Utility;
using Samsonite.Library.Web.Business.Custom.Models;
using Samsonite.Library.Web.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace Samsonite.Library.Web.Business.Custom
{
    public class OMSAPIToolsService : IOMSAPIToolsService
    {
        private IBaseService _baseService;
        private appEntities _appDB;
        public OMSAPIToolsService(IBaseService baseService, appEntities appEntities)
        {
            _baseService = baseService;
            _appDB = appEntities;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public List<View_ApplicationApi> GetQuery()
        {
            List<View_ApplicationApi> _result = new List<View_ApplicationApi>();
            var _list = _appDB.View_ApplicationApi.AsQueryable();

            //搜索条件
            _list = _list.Where(p => !p.IsLock);

            //返回数据
            _result = _list.ToList();
            return _result;
        }

        #region 韩国
        /// <summary>
        /// 仓库API参数
        /// </summary>
        /// <returns></returns>
        public OMSApiToolInitializeResponse GetKrWMSParams()
        {
            OMSApiToolInitializeResponse _result = new OMSApiToolInitializeResponse();
            //读取API信息
            int _apiID = 1;
            var _info = _appDB.ApplicationApiDetail.Where(p => p.ID == _apiID).SingleOrDefault();
            if (_info != null)
            {
                _result.AccessToken = _info.AccessToken;
                _result.APIInterfaces = new List<OMSApiToolInitializeInterface>()
                {
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=1,
                        APIPath = "OMS.Warehouse.GetOrders",
                        RequestURL = $"{_info.InterfaceAddress}/GetOrders",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="startDate",
                                Value=DateTime.Now.AddDays(-7).ToString("yyyyMMdd000000"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="endDate",
                                Value=DateTime.Now.ToString("yyyyMMdd235959"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=2,
                        APIPath = "OMS.Warehouse.GetChangedOrders",
                        RequestURL = $"{_info.InterfaceAddress}/GetChangedOrders",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="startDate",
                                Value=DateTime.Now.AddDays(-7).ToString("yyyyMMdd000000"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="endDate",
                                Value=DateTime.Now.ToString("yyyyMMdd235959"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=3,
                        APIPath = "OMS.Warehouse.PostReply",
                        RequestURL = $"{_info.InterfaceAddress}/PostReply",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new KoreaApiToolRequest.PostReplyRequest()
                            {
                                 OrderNo="",
                                 SubOrderId="",
                                 Type=0,
                                 ReplyDate=DateTime.Now.ToString("yyyyMMddHHmmss"),
                                 ReplyState=(int)WarehouseStatus.DealSuccessful,
                                 Message="",
                                 Id=0
                            }
                        }
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=4,
                        APIPath = "OMS.Warehouse.PostDelivery",
                        RequestURL = $"{_info.InterfaceAddress}/PostDelivery",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new KoreaApiToolRequest.PostDeliveryRequest()
                            {
                                 MallId="",
                                 OrderNo = "",
                                 SubOrderId="",
                                 DeliveryNo=$"N{DateTime.Now.ToString("yyyyMMdd")}001",
                                 ReceiveCost="0.0",
                                 Type="",
                                 Company="한진",
                                 DeliveryCode="delv0010",
                                 Sku="",
                                 ReceiveDate=DateTime.Now.ToString("yyyyMMddHHmmss"),
                                 Quantity=1,
                                 Message="ok",
                                 Receive="김용환",
                                 Address="서울특별시 송파구 오금로23길 30 (방이동) 302호",
                                 Zipcode="056-33",
                                 Warehouse="JB물류",
                            }
                        }
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=5,
                        APIPath = "OMS.Warehouse.PostInventory",
                        RequestURL = $"{_info.InterfaceAddress}/PostInventory",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new KoreaApiToolRequest.PostInventoryRequest()
                            {
                                 Sku="",
                                 ProductId="",
                                 Quantity=1
                            }
                        }
                     }
                };
            }
            return _result;
        }

        /// <summary>
        /// API请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OMSApiResultResponse GetKrApiResult(OMSApiToolRequest request)
        {
            string _encryptionKey = "asdf#$^%CZC232332ADSF323&*234df/";
            try
            {
                OMSApiResultResponse _result = new OMSApiResultResponse();

                if (request.apiPath < 1 || request.apiPath > 5)
                {
                    throw new Exception("Please select a API Path!");
                }

                if (request.httpMethod == HttpMethod.POST.ToString())
                {
                    if (string.IsNullOrEmpty(request.postData))
                    {
                        throw new Exception("Please input a data!");
                    }
                }

                //参数
                var _subParams = JsonSerializer.Deserialize<List<CommomParamRequest>>(request.paramArray);
                //GET
                if (request.httpMethod.ToUpper().Equals(HttpMethod.GET.ToString()))
                {
                    IDictionary<string, string> _params = new Dictionary<string, string>();
                    //默认参数
                    _params.Add("token", request.AccessToken);
                    //传递参数
                    foreach (var item in _subParams)
                    {
                        _params.Add(item.Param, item.Value.ToString());
                    }
                    //发起请求
                    var _res = this.DoGet(request.requestURL, _params);
                    _result.RequestInfo = $"{request.httpMethod.ToUpper()} {_res.RequestUrl}";
                    _result.ResponseInfo = VariableHelper.ConvertJsonString(AESEncryption.AesDecrypt(_res.ResponseInfo, _encryptionKey));
                }
                //POST
                else if (request.httpMethod.ToUpper().Equals(HttpMethod.POST.ToString()))
                {
                    IDictionary<string, string> _params = new Dictionary<string, string>();
                    //默认参数
                    _params.Add("token", request.AccessToken);
                    //发起请求
                    var _res = this.DoPost(request.requestURL, _params, request.postData);
                    _result.RequestInfo = $"{request.httpMethod.ToUpper()} {_res.RequestUrl}\r\n{VariableHelper.ConvertJsonString(_res.RequestData)}";
                    _result.ResponseInfo = VariableHelper.ConvertJsonString(_res.ResponseInfo);
                }

                //返回信息
                _result.Result = true;
                _result.Message = string.Empty;
                return _result;
            }
            catch (Exception ex)
            {
                //返回信息
                return new OMSApiResultResponse()
                {
                    Result = false,
                    Message = ex.Message,
                    RequestInfo = "",
                    ResponseInfo = ""
                };
            }
        }
        #endregion

        #region 泰国
        /// <summary>
        /// 仓库API参数
        /// </summary>
        /// <returns></returns>
        public OMSApiToolInitializeResponse GetThWMSParams()
        {
            OMSApiToolInitializeResponse _result = new OMSApiToolInitializeResponse();
            //读取API信息
            int _apiID = 2;
            var _info = _appDB.ApplicationApiDetail.Where(p => p.ID == _apiID).SingleOrDefault();
            if (_info != null)
            {
                _result.UserID = _info.UserID;
                _result.APIInterfaces = new List<OMSApiToolInitializeInterface>()
                {
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=1,
                        APIPath = "OMS.Warehouse.GetOrders",
                        RequestURL = $"{_info.InterfaceAddress}/GetOrders",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="startDate",
                                Value=DateTime.Now.AddDays(-7).ToString("yyyyMMdd000000"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="endDate",
                                Value=DateTime.Now.ToString("yyyyMMdd235959"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="orderBy",
                                Value="asc",
                                Placeholder="asc/desc",
                                Required=false
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=2,
                        APIPath = "OMS.Warehouse.GetChangedOrders",
                        RequestURL = $"{_info.InterfaceAddress}/GetChangedOrders",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="startDate",
                                Value=DateTime.Now.AddDays(-7).ToString("yyyyMMdd000000"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="endDate",
                                Value=DateTime.Now.ToString("yyyyMMdd235959"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="orderBy",
                                Value="asc",
                                Placeholder="asc/desc",
                                Required=false
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=3,
                        APIPath = "OMS.Warehouse.PostReply",
                        RequestURL = $"{_info.InterfaceAddress}/PostReply",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new ThailandApiToolRequest.PostReplyRequest()
                            {
                                 MallCode="",
                                 OrderNo="",
                                 SubOrderNo="",
                                 Type=0,
                                 ReplyDate=DateTime.Now.ToString("yyyyMMddHHmmss"),
                                 ReplyState=(int)WarehouseStatus.DealSuccessful,
                                 Message="",
                                 RecordId=0
                            }
                        }
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=4,
                        APIPath = "OMS.Warehouse.PostInventory",
                        RequestURL = $"{_info.InterfaceAddress}/PostInventory",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new ThailandApiToolRequest.PostInventoryRequest()
                            {
                                 Sku="",
                                 ProductType=1,
                                 ProductId="",
                                 Quantity=1
                            }
                        }
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=5,
                        APIPath = "OMS.Warehouse.PostDetail",
                        RequestURL = $"{_info.InterfaceAddress}/PostDetail",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new ThailandApiToolRequest.PostDetailRequest()
                            {
                                MallCode="",
                                OrderNo="",
                                SubOrderNo="",
                                Type=1,
                                Data=new MalaysiaApiToolRequest.PostShippingStatusResponse()
                                {
                                     Status="PICKED"
                                }
                            }
                        }
                    }
                };
            }
            return _result;
        }

        /// <summary>
        /// 平台API参数
        /// </summary>
        /// <returns></returns>
        public OMSApiToolInitializeResponse GetThPlatformParams()
        {
            OMSApiToolInitializeResponse _result = new OMSApiToolInitializeResponse();
            //读取API信息
            int _apiID = 4;
            var _info = _appDB.ApplicationApiDetail.Where(p => p.ID == _apiID).SingleOrDefault();
            if (_info != null)
            {
                _result.UserID = _info.UserID;
                _result.APIInterfaces = new List<OMSApiToolInitializeInterface>()
                {
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=1,
                        APIPath = "OMS.Platform.Stores.Get",
                        RequestURL = $"{_info.InterfaceAddress}/platform/stores/get",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="storeSapCode",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=false
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    }
                };
            }
            return _result;
        }

        /// <summary>
        /// API请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OMSApiResultResponse GetThApiResult(OMSApiToolRequest request)
        {
            try
            {
                OMSApiResultResponse _result = new OMSApiResultResponse();

                if (request.apiPath < 1 || request.apiPath > 5)
                {
                    throw new Exception("Please select a API Path!");
                }

                if (request.httpMethod == HttpMethod.POST.ToString())
                {
                    if (string.IsNullOrEmpty(request.postData))
                    {
                        throw new Exception("Please input a data!");
                    }
                }

                //密钥
                var _secret = this.GetAccessToken(request.ApiID);

                //参数
                var _subParams = JsonSerializer.Deserialize<List<CommomParamRequest>>(request.paramArray);
                //GET
                if (request.httpMethod.ToUpper().Equals(HttpMethod.GET.ToString()))
                {
                    IDictionary<string, string> _params = new Dictionary<string, string>();
                    //默认参数
                    _params.Add("userid", request.UserID);
                    _params.Add("version", request.Version);
                    _params.Add("format", request.Format);
                    _params.Add("method", request.Method);
                    _params.Add("timestamp", TimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString());
                    //传递参数
                    foreach (var item in _subParams)
                    {
                        _params.Add(item.Param, item.Value.ToString());
                    }
                    //签名
                    _params.Add("sign", CreateSign(_params, _secret, request.Method));
                    //发起请求
                    var _res = this.DoGet(request.requestURL, _params);
                    _result.RequestInfo = $"{request.httpMethod.ToUpper()} {_res.RequestUrl}";
                    _result.ResponseInfo = VariableHelper.ConvertJsonString(_res.ResponseInfo);
                }
                //POST
                else if (request.httpMethod.ToUpper().Equals(HttpMethod.POST.ToString()))
                {
                    IDictionary<string, string> _params = new Dictionary<string, string>();
                    //默认参数
                    _params.Add("userid", request.UserID);
                    _params.Add("version", request.Version);
                    _params.Add("format", request.Format);
                    _params.Add("method", request.Method);
                    _params.Add("timestamp", TimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString());
                    //签名
                    _params.Add("sign", CreateSign(_params, _secret, request.Method));
                    //发起请求
                    var _res = this.DoPost(request.requestURL, _params, request.postData);
                    _result.RequestInfo = $"{request.httpMethod.ToUpper()} {_res.RequestUrl}\r\n{VariableHelper.ConvertJsonString(_res.RequestData)}";
                    _result.ResponseInfo = VariableHelper.ConvertJsonString(_res.ResponseInfo);
                }

                //返回信息
                _result.Result = true;
                _result.Message = string.Empty;
                return _result;
            }
            catch (Exception ex)
            {
                //返回信息
                return new OMSApiResultResponse()
                {
                    Result = false,
                    Message = ex.Message,
                    RequestInfo = "",
                    ResponseInfo = ""
                };
            }
        }
        #endregion

        #region 香港Tumi
        /// <summary>
        /// 仓库API参数
        /// </summary>
        /// <returns></returns>
        public OMSApiToolInitializeResponse GetHkTumiWMSParams()
        {
            OMSApiToolInitializeResponse _result = new OMSApiToolInitializeResponse();
            //读取API信息
            int _apiID = 10;
            var _info = _appDB.ApplicationApiDetail.Where(p => p.ID == _apiID).SingleOrDefault();
            if (_info != null)
            {
                _result.UserID = _info.UserID;
                _result.APIInterfaces = new List<OMSApiToolInitializeInterface>()
                {
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=1,
                        APIPath = "OMS.Warehouse.GetOrders",
                        RequestURL = $"{_info.InterfaceAddress}/GetOrders",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="startDate",
                                Value=DateTime.Now.AddDays(-7).ToString("yyyyMMdd000000"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="endDate",
                                Value=DateTime.Now.ToString("yyyyMMdd235959"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="orderBy",
                                Value="asc",
                                Placeholder="asc/desc",
                                Required=false
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=2,
                        APIPath = "OMS.Warehouse.GetChangedOrders",
                        RequestURL = $"{_info.InterfaceAddress}/GetChangedOrders",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="startDate",
                                Value=DateTime.Now.AddDays(-7).ToString("yyyyMMdd000000"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="endDate",
                                Value=DateTime.Now.ToString("yyyyMMdd235959"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="orderBy",
                                Value="asc",
                                Placeholder="asc/desc",
                                Required=false
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=3,
                        APIPath = "OMS.Warehouse.PostReply",
                        RequestURL = $"{_info.InterfaceAddress}/PostReply",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new HongkongTumiApiToolRequest.PostReplyRequest()
                            {
                                 MallCode="",
                                 OrderNo="",
                                 SubOrderNo="",
                                 Type=0,
                                 ReplyDate=DateTime.Now.ToString("yyyyMMddHHmmss"),
                                 ReplyState=(int)WarehouseStatus.DealSuccessful,
                                 Message="",
                                 RecordId=0
                            }
                        }
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=4,
                        APIPath = "OMS.Warehouse.PostDelivery",
                        RequestURL = $"{_info.InterfaceAddress}/PostDelivery",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new HongkongTumiApiToolRequest.PostDeliveryRequest()
                            {
                                MallCode="",
                                OrderNo = "",
                                SubOrderNo="",
                                Sku="",
                                DeliveryCode="",
                                Company="SF",
                                DeliveryNo=$"SF{DateTime.Now.ToString("yyyyMMdd")}001",
                                Packages=1,
                                Type="",
                                ReceiveCost=0,
                                Warehouse="",
                                ReceiveDate=DateTime.Now.ToString("yyyyMMddHHmmss"),
                                DealDate=DateTime.Now.ToString("yyyyMMddHHmmss"),
                                SendDate=DateTime.Now.ToString("yyyyMMddHHmmss")
                            }
                        }
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=5,
                        APIPath = "OMS.Warehouse.PostInventory",
                        RequestURL = $"{_info.InterfaceAddress}/PostInventory",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new HongkongTumiApiToolRequest.PostInventoryRequest()
                            {
                                 Sku="",
                                 ProductType=1,
                                 ProductId="",
                                 Quantity=1
                            }
                        }
                    }
                };
            }
            return _result;
        }

        /// <summary>
        /// API请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OMSApiResultResponse GetHkTumiApiResult(OMSApiToolRequest request)
        {
            try
            {
                OMSApiResultResponse _result = new OMSApiResultResponse();

                if (request.apiPath < 1 || request.apiPath > 5)
                {
                    throw new Exception("Please select a API Path!");
                }

                if (request.httpMethod == HttpMethod.POST.ToString())
                {
                    if (string.IsNullOrEmpty(request.postData))
                    {
                        throw new Exception("Please input a data!");
                    }
                }

                //密钥
                var _secret = this.GetAccessToken(request.ApiID);

                //参数
                var _subParams = JsonSerializer.Deserialize<List<CommomParamRequest>>(request.paramArray);
                //GET
                if (request.httpMethod.ToUpper().Equals(HttpMethod.GET.ToString()))
                {
                    IDictionary<string, string> _params = new Dictionary<string, string>();
                    //默认参数
                    _params.Add("userid", request.UserID);
                    _params.Add("version", request.Version);
                    _params.Add("format", request.Format);
                    _params.Add("method", request.Method);
                    _params.Add("timestamp", TimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString());
                    //传递参数
                    foreach (var item in _subParams)
                    {
                        _params.Add(item.Param, item.Value.ToString());
                    }
                    //签名
                    _params.Add("sign", CreateSign(_params, _secret, request.Method));
                    //发起请求
                    var _res = this.DoGet(request.requestURL, _params);
                    _result.RequestInfo = $"{request.httpMethod.ToUpper()} {_res.RequestUrl}";
                    _result.ResponseInfo = VariableHelper.ConvertJsonString(_res.ResponseInfo);
                }
                //POST
                else if (request.httpMethod.ToUpper().Equals(HttpMethod.POST.ToString()))
                {
                    IDictionary<string, string> _params = new Dictionary<string, string>();
                    //默认参数
                    _params.Add("userid", request.UserID);
                    _params.Add("version", request.Version);
                    _params.Add("format", request.Format);
                    _params.Add("method", request.Method);
                    _params.Add("timestamp", TimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString());
                    //签名
                    _params.Add("sign", CreateSign(_params, _secret, request.Method));
                    //发起请求
                    var _res = this.DoPost(request.requestURL, _params, request.postData);
                    _result.RequestInfo = $"{request.httpMethod.ToUpper()} {_res.RequestUrl}\r\n{VariableHelper.ConvertJsonString(_res.RequestData)}";
                    _result.ResponseInfo = VariableHelper.ConvertJsonString(_res.ResponseInfo);
                }

                //返回信息
                _result.Result = true;
                _result.Message = string.Empty;
                return _result;
            }
            catch (Exception ex)
            {
                //返回信息
                return new OMSApiResultResponse()
                {
                    Result = false,
                    Message = ex.Message,
                    RequestInfo = "",
                    ResponseInfo = ""
                };
            }
        }
        #endregion

        #region 马来西亚
        /// <summary>
        /// 仓库API参数
        /// </summary>
        /// <returns></returns>
        public OMSApiToolInitializeResponse GetMyWMSParams()
        {
            OMSApiToolInitializeResponse _result = new OMSApiToolInitializeResponse();
            //读取API信息
            int _apiID = 13;
            var _info = _appDB.ApplicationApiDetail.Where(p => p.ID == _apiID).SingleOrDefault();
            if (_info != null)
            {
                _result.UserID = _info.UserID;
                _result.APIInterfaces = new List<OMSApiToolInitializeInterface>()
                {
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=1,
                        APIPath = "OMS.Warehouse.GetOrders",
                        RequestURL = $"{_info.InterfaceAddress}/GetOrders",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="startDate",
                                Value=DateTime.Now.AddDays(-7).ToString("yyyyMMdd000000"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="endDate",
                                Value=DateTime.Now.ToString("yyyyMMdd235959"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="orderBy",
                                Value="asc",
                                Placeholder="asc/desc",
                                Required=false
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=2,
                        APIPath = "OMS.Warehouse.GetChangedOrders",
                        RequestURL = $"{_info.InterfaceAddress}/GetChangedOrders",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="startDate",
                                Value=DateTime.Now.AddDays(-7).ToString("yyyyMMdd000000"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="endDate",
                                Value=DateTime.Now.ToString("yyyyMMdd235959"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="orderBy",
                                Value="asc",
                                Placeholder="asc/desc",
                                Required=false
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=3,
                        APIPath = "OMS.Warehouse.PostReply",
                        RequestURL = $"{_info.InterfaceAddress}/PostReply",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new MalaysiaApiToolRequest.PostReplyRequest()
                            {
                                 MallCode="",
                                 OrderNo="",
                                 SubOrderId="",
                                 Type=0,
                                 ReplyDate=DateTime.Now.ToString("yyyyMMddHHmmss"),
                                 ReplyState=(int)WarehouseStatus.DealSuccessful,
                                 Message="",
                                 RecordId=0
                            }
                        }
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=4,
                        APIPath = "OMS.Warehouse.PostInventory",
                        RequestURL = $"{_info.InterfaceAddress}/PostInventory",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new MalaysiaApiToolRequest.PostInventoryRequest()
                            {
                                 PlantCode="",
                                 Sku="",
                                 ProductType=1,
                                 ProductId="",
                                 Quantity=1
                            }
                        }
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=5,
                        APIPath = "OMS.Warehouse.PostDetail",
                        RequestURL = $"{_info.InterfaceAddress}/PostDetail",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new MalaysiaApiToolRequest.PostDetailRequest()
                            {
                                MallCode="",
                                OrderNo="",
                                SubOrderId="",
                                Type=1,
                                ProductId="",
                                Sku="",
                                Quantity=1,
                                Data=new MalaysiaApiToolRequest.PostShippingStatusResponse()
                                {
                                     Status="PICKED"
                                }
                            }
                        }
                    }
                };
            }
            return _result;
        }

        /// <summary>
        /// 平台API参数
        /// </summary>
        /// <returns></returns>
        public OMSApiToolInitializeResponse GetMyPlatformParams()
        {
            OMSApiToolInitializeResponse _result = new OMSApiToolInitializeResponse();
            //读取API信息
            int _apiID = 15;
            var _info = _appDB.ApplicationApiDetail.Where(p => p.ID == _apiID).SingleOrDefault();
            if (_info != null)
            {
                _result.UserID = _info.UserID;
                _result.APIInterfaces = new List<OMSApiToolInitializeInterface>()
                {
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=1,
                        APIPath = "OMS.Platform.Stores.Get",
                        RequestURL = $"{_info.InterfaceAddress}/platform/stores/get",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="storeSapCode",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=false
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    //new OMSApiToolInitializeInterface()
                    //{
                    //    APIPathID=2,
                    //    APIPath = "OMS.Platform.Post.Orders",
                    //    RequestURL = $"{_info.InterfaceAddress}/platform/order/post",
                    //    HttpMethod = HttpMethod.POST.ToString(),
                    //    Parameters=new List<CommomParamRequest>(),
                    //    PostDatas=new List<object>()
                    //    {
                    //        new object()
                    //        {

                    //        }
                    //    }
                    //},
                    //new OMSApiToolInitializeInterface()
                    //{
                    //    APIPathID=3,
                    //    APIPath = "OMS.Platform.Order.Details",
                    //    RequestURL = $"{_info.InterfaceAddress}/platform/order/details",
                    //    HttpMethod = HttpMethod.GET.ToString(),
                    //    Parameters = new List<CommomParamRequest>()
                    //    {
                    //        new CommomParamRequest()
                    //        {
                    //            Param="storeSapCode",
                    //            Value=string.Empty,
                    //            Placeholder=string.Empty,
                    //            Required=true
                    //        },
                    //        new CommomParamRequest()
                    //        {
                    //            Param="orderNos",
                    //            Value=string.Empty,
                    //            Placeholder=string.Empty,
                    //            Required=true
                    //        }
                    //    },
                    //    PostDatas=new List<object>()
                    //},
                    //new OMSApiToolInitializeInterface()
                    //{
                    //    APIPathID=4,
                    //    APIPath = "OMS.Platform.Inventorys.Get",
                    //    RequestURL = $"{_info.InterfaceAddress}/platform/inventorys/get",
                    //    HttpMethod = HttpMethod.GET.ToString(),
                    //    Parameters = new List<CommomParamRequest>()
                    //    {
                    //        new CommomParamRequest()
                    //        {
                    //            Param="storeSapCode",
                    //            Value=string.Empty,
                    //            Placeholder=string.Empty,
                    //            Required=true
                    //        },
                    //        new CommomParamRequest()
                    //        {
                    //            Param="productIds",
                    //            Value=string.Empty,
                    //            Placeholder=string.Empty,
                    //            Required=false
                    //        },
                    //        new CommomParamRequest()
                    //        {
                    //            Param="pageIndex",
                    //            Value=1,
                    //            Placeholder="1",
                    //            Required=true
                    //        },
                    //        new CommomParamRequest()
                    //        {
                    //            Param="pageSize",
                    //            Value=50,
                    //            Placeholder="50",
                    //            Required=true
                    //        }
                    //    },
                    //    PostDatas=new List<object>()
                    //},
                    //new OMSApiToolInitializeInterface()
                    //{
                    //    APIPathID=5,
                    //    APIPath = "OMS.Platform.Prices.Get",
                    //    RequestURL = $"{_info.InterfaceAddress}/platform/prices/get",
                    //    HttpMethod = HttpMethod.GET.ToString(),
                    //    Parameters = new List<CommomParamRequest>()
                    //    {
                    //        new CommomParamRequest()
                    //        {
                    //            Param="storeSapCode",
                    //            Value=string.Empty,
                    //            Placeholder=string.Empty,
                    //            Required=true
                    //        },
                    //        new CommomParamRequest()
                    //        {
                    //            Param="productIds",
                    //            Value=string.Empty,
                    //            Placeholder=string.Empty,
                    //            Required=false
                    //        },
                    //        new CommomParamRequest()
                    //        {
                    //            Param="pageIndex",
                    //            Value=1,
                    //            Placeholder="1",
                    //            Required=true
                    //        },
                    //        new CommomParamRequest()
                    //        {
                    //            Param="pageSize",
                    //            Value=50,
                    //            Placeholder="50",
                    //            Required=true
                    //        }
                    //    },
                    //    PostDatas=new List<object>()
                    //},
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=2,
                        APIPath = "OMS.Platform.Express.Status",
                        RequestURL = $"{_info.InterfaceAddress}/platform/express/status",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="storeSapCode",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="trackingNos",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    }
                };
            }
            return _result;
        }

        /// <summary>
        /// API请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OMSApiResultResponse GetMyApiResult(OMSApiToolRequest request)
        {
            try
            {
                OMSApiResultResponse _result = new OMSApiResultResponse();

                if (request.apiPath < 1 || request.apiPath > 7)
                {
                    throw new Exception("Please select a API Path!");
                }

                if (request.httpMethod == HttpMethod.POST.ToString())
                {
                    if (string.IsNullOrEmpty(request.postData))
                    {
                        throw new Exception("Please input a data!");
                    }
                }

                //密钥
                var _secret = this.GetAccessToken(request.ApiID);

                //参数
                var _subParams = JsonSerializer.Deserialize<List<CommomParamRequest>>(request.paramArray);
                //GET
                if (request.httpMethod.ToUpper().Equals(HttpMethod.GET.ToString()))
                {
                    IDictionary<string, string> _params = new Dictionary<string, string>();
                    //默认参数
                    _params.Add("userid", request.UserID);
                    _params.Add("version", request.Version);
                    _params.Add("format", request.Format);
                    _params.Add("method", request.Method);
                    _params.Add("timestamp", TimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString());
                    //传递参数
                    foreach (var item in _subParams)
                    {
                        _params.Add(item.Param, item.Value.ToString());
                    }
                    //签名
                    _params.Add("sign", CreateSign(_params, _secret, request.Method));
                    //发起请求
                    var _res = this.DoGet(request.requestURL, _params);
                    _result.RequestInfo = $"{request.httpMethod.ToUpper()} {_res.RequestUrl}";
                    _result.ResponseInfo = VariableHelper.ConvertJsonString(_res.ResponseInfo);
                }
                //POST
                else if (request.httpMethod.ToUpper().Equals(HttpMethod.POST.ToString()))
                {
                    IDictionary<string, string> _params = new Dictionary<string, string>();
                    //默认参数
                    _params.Add("userid", request.UserID);
                    _params.Add("version", request.Version);
                    _params.Add("format", request.Format);
                    _params.Add("method", request.Method);
                    _params.Add("timestamp", TimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString());
                    //签名
                    _params.Add("sign", CreateSign(_params, _secret, request.Method));
                    //发起请求
                    var _res = this.DoPost(request.requestURL, _params, request.postData);
                    _result.RequestInfo = $"{request.httpMethod.ToUpper()} {_res.RequestUrl}\r\n{VariableHelper.ConvertJsonString(_res.RequestData)}";
                    _result.ResponseInfo = VariableHelper.ConvertJsonString(_res.ResponseInfo);
                }

                //返回信息
                _result.Result = true;
                _result.Message = string.Empty;
                return _result;
            }
            catch (Exception ex)
            {
                //返回信息
                return new OMSApiResultResponse()
                {
                    Result = false,
                    Message = ex.Message,
                    RequestInfo = "",
                    ResponseInfo = ""
                };
            }
        }
        #endregion

        #region 日本
        /// <summary>
        /// 仓库API参数
        /// </summary>
        /// <returns></returns>
        public OMSApiToolInitializeResponse GetJpnWMSParams()
        {
            OMSApiToolInitializeResponse _result = new OMSApiToolInitializeResponse();
            //读取API信息
            int _apiID = 16;
            var _info = _appDB.ApplicationApiDetail.Where(p => p.ID == _apiID).SingleOrDefault();
            if (_info != null)
            {
                _result.UserID = _info.UserID;
                _result.APIInterfaces = new List<OMSApiToolInitializeInterface>()
                {
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=1,
                        APIPath = "OMS.Warehouse.GetOrders",
                        RequestURL = $"{_info.InterfaceAddress}/GetOrders",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="startDate",
                                Value=DateTime.Now.AddDays(-7).ToString("yyyyMMdd000000"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="endDate",
                                Value=DateTime.Now.ToString("yyyyMMdd235959"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="orderBy",
                                Value="asc",
                                Placeholder="asc/desc",
                                Required=false
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=2,
                        APIPath = "OMS.Warehouse.GetChangedOrders",
                        RequestURL = $"{_info.InterfaceAddress}/GetChangedOrders",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="startDate",
                                Value=DateTime.Now.AddDays(-7).ToString("yyyyMMdd000000"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="endDate",
                                Value=DateTime.Now.ToString("yyyyMMdd235959"),
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="orderBy",
                                Value="asc",
                                Placeholder="asc/desc",
                                Required=false
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=3,
                        APIPath = "OMS.Warehouse.PostReply",
                        RequestURL = $"{_info.InterfaceAddress}/PostReply",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new JapanTumiApiToolRequest.PostReplyRequest()
                            {
                                 MallCode="1197417",
                                 OrderNo="",
                                 SubOrderNo="",
                                 Type=0,
                                 ReplyDate=DateTime.Now.ToString("yyyyMMddHHmmss"),
                                 ReplyState=(int)WarehouseStatus.DealSuccessful,
                                 Message="",
                                 RecordId=0
                            }
                        }
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=4,
                        APIPath = "OMS.Warehouse.PostDelivery",
                        RequestURL = $"{_info.InterfaceAddress}/PostDelivery",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new JapanTumiApiToolRequest.PostDeliveryRequest()
                            {
                                MallCode="1197417",
                                OrderNo = "",
                                SubOrderNo="",
                                Sku="",
                                DeliveryCode="",
                                Company="SAGAWA",
                                //DeliveryNo=$"SAGAWA{DateTime.Now.ToString("yyyyMMdd")}001",
                                DeliveryNo="490657716010",
                                Packages=1,
                                Type="",
                                ReceiveCost=0,
                                Warehouse="",
                                ReceiveDate=DateTime.Now.ToString("yyyyMMddHHmmss"),
                                DealDate=DateTime.Now.ToString("yyyyMMddHHmmss"),
                                SendDate=DateTime.Now.ToString("yyyyMMddHHmmss")
                            }
                        }
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=5,
                        APIPath = "OMS.Warehouse.PostInventory",
                        RequestURL = $"{_info.InterfaceAddress}/PostInventory",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new JapanTumiApiToolRequest.PostInventoryRequest()
                            {
                                 Sku="",
                                 ProductType=1,
                                 ProductId="",
                                 Quantity=1
                            }
                        }
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=6,
                        APIPath = "OMS.Warehouse.UpdateShipmentStatus",
                        RequestURL = $"{_info.InterfaceAddress}/UpdateShipmentStatus",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new JapanTumiApiToolRequest.UpdateShipmentStatusRequest()
                            {
                                  DeliveryNo="",
                                  DeliveryCompany="",
                                  UpdateDate=DateTime.Now.ToString("yyyyMMddHHmmss"),
                                  Status="SIGN",
                                  Remark=""
                            }
                        }
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=7,
                        APIPath = "OMS.Warehouse.UpdateWMSStatus",
                        RequestURL = $"{_info.InterfaceAddress}/UpdateWMSStatus",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new JapanTumiApiToolRequest.UpdateWMSStatusRequest()
                            {
                                 MallCode="1197417",
                                 OrderNo="",
                                 SubOrderNo="",
                                 Status="PICKED",
                                 Remark=""
                            }
                        }
                    }
                };
            }
            return _result;
        }

        /// <summary>
        /// 平台API参数
        /// </summary>
        /// <returns></returns>
        public OMSApiToolInitializeResponse GetJpnPlatformParams()
        {
            OMSApiToolInitializeResponse _result = new OMSApiToolInitializeResponse();
            //读取API信息
            int _apiID = 17;
            var _info = _appDB.ApplicationApiDetail.Where(p => p.ID == _apiID).SingleOrDefault();
            if (_info != null)
            {
                _result.UserID = _info.UserID;
                _result.APIInterfaces = new List<OMSApiToolInitializeInterface>()
                {
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=1,
                        APIPath = "OMS.Platform.Stores.Get",
                        RequestURL = $"{_info.InterfaceAddress}/platform/stores/get",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="storeSapCode",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=false
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=2,
                        APIPath = "OMS.Platform.Post.Orders",
                        RequestURL = $"{_info.InterfaceAddress}/platform/order/post",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new object()
                            {

                            }
                        }
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=3,
                        APIPath = "OMS.Platform.Order.Details",
                        RequestURL = $"{_info.InterfaceAddress}/platform/order/details",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="storeSapCode",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="orderNos",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=4,
                        APIPath = "OMS.Platform.Inventorys.Get",
                        RequestURL = $"{_info.InterfaceAddress}/platform/inventorys/get",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="storeSapCode",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="productIds",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=false
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=5,
                        APIPath = "OMS.Platform.Prices.Get",
                        RequestURL = $"{_info.InterfaceAddress}/platform/prices/get",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="storeSapCode",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="productIds",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=false
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=6,
                        APIPath = "OMS.Platform.Express.Status",
                        RequestURL = $"{_info.InterfaceAddress}/platform/express/status",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="storeSapCode",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="trackingNos",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    }
                };
            }
            return _result;
        }

        /// <summary>
        /// API请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OMSApiResultResponse GetJpnApiResult(OMSApiToolRequest request)
        {
            try
            {
                OMSApiResultResponse _result = new OMSApiResultResponse();

                if (request.apiPath < 1 || request.apiPath > 7)
                {
                    throw new Exception("Please select a API Path!");
                }

                if (request.httpMethod == HttpMethod.POST.ToString())
                {
                    if (string.IsNullOrEmpty(request.postData))
                    {
                        throw new Exception("Please input a data!");
                    }
                }

                //密钥
                var _secret = this.GetAccessToken(request.ApiID);

                //参数
                var _subParams = JsonSerializer.Deserialize<List<CommomParamRequest>>(request.paramArray);
                //GET
                if (request.httpMethod.ToUpper().Equals(HttpMethod.GET.ToString()))
                {
                    IDictionary<string, string> _params = new Dictionary<string, string>();
                    //默认参数
                    _params.Add("userid", request.UserID);
                    _params.Add("version", request.Version);
                    _params.Add("format", request.Format);
                    _params.Add("method", request.Method);
                    _params.Add("timestamp", TimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString());
                    //传递参数
                    foreach (var item in _subParams)
                    {
                        _params.Add(item.Param, item.Value.ToString());
                    }
                    //签名
                    _params.Add("sign", CreateSign(_params, _secret, request.Method));
                    //发起请求
                    var _res = this.DoGet(request.requestURL, _params);
                    _result.RequestInfo = $"{request.httpMethod.ToUpper()} {_res.RequestUrl}";
                    _result.ResponseInfo = VariableHelper.ConvertJsonString(_res.ResponseInfo);
                }
                //POST
                else if (request.httpMethod.ToUpper().Equals(HttpMethod.POST.ToString()))
                {
                    IDictionary<string, string> _params = new Dictionary<string, string>();
                    //默认参数
                    _params.Add("userid", request.UserID);
                    _params.Add("version", request.Version);
                    _params.Add("format", request.Format);
                    _params.Add("method", request.Method);
                    _params.Add("timestamp", TimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString());
                    //签名
                    _params.Add("sign", CreateSign(_params, _secret, request.Method));
                    //发起请求
                    var _res = this.DoPost(request.requestURL, _params, request.postData);
                    _result.RequestInfo = $"{request.httpMethod.ToUpper()} {_res.RequestUrl}\r\n{VariableHelper.ConvertJsonString(_res.RequestData)}";
                    _result.ResponseInfo = VariableHelper.ConvertJsonString(_res.ResponseInfo);
                }

                //返回信息
                _result.Result = true;
                _result.Message = string.Empty;
                return _result;
            }
            catch (Exception ex)
            {
                //返回信息
                return new OMSApiResultResponse()
                {
                    Result = false,
                    Message = ex.Message,
                    RequestInfo = "",
                    ResponseInfo = ""
                };
            }
        }
        #endregion

        #region 台湾
        /// <summary>
        /// 平台API参数
        /// </summary>
        /// <returns></returns>
        public OMSApiToolInitializeResponse GetTwPlatformParams()
        {
            OMSApiToolInitializeResponse _result = new OMSApiToolInitializeResponse();
            //读取API信息
            int _apiID = 18;
            var _info = _appDB.ApplicationApiDetail.Where(p => p.ID == _apiID).SingleOrDefault();
            if (_info != null)
            {
                _result.UserID = _info.UserID;
                _result.APIInterfaces = new List<OMSApiToolInitializeInterface>()
                {
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=1,
                        APIPath = "OMS.Platform.Stores.Get",
                        RequestURL = $"{_info.InterfaceAddress}/platform/stores/get",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="storeSapCode",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=false
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=2,
                        APIPath = "OMS.Platform.Post.Orders",
                        RequestURL = $"{_info.InterfaceAddress}/platform/order/post",
                        HttpMethod = HttpMethod.POST.ToString(),
                        Parameters=new List<CommomParamRequest>(),
                        PostDatas=new List<object>()
                        {
                            new object()
                            {

                            }
                        }
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=3,
                        APIPath = "OMS.Platform.Order.Details",
                        RequestURL = $"{_info.InterfaceAddress}/platform/order/details",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="storeSapCode",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="orderNos",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=4,
                        APIPath = "OMS.Platform.Inventorys.Get",
                        RequestURL = $"{_info.InterfaceAddress}/platform/inventorys/get",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="storeSapCode",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="productIds",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=false
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=5,
                        APIPath = "OMS.Platform.Prices.Get",
                        RequestURL = $"{_info.InterfaceAddress}/platform/prices/get",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="storeSapCode",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="productIds",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=false
                            },
                            new CommomParamRequest()
                            {
                                Param="pageIndex",
                                Value=1,
                                Placeholder="1",
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="pageSize",
                                Value=50,
                                Placeholder="50",
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    },
                    new OMSApiToolInitializeInterface()
                    {
                        APIPathID=6,
                        APIPath = "OMS.Platform.Express.Status",
                        RequestURL = $"{_info.InterfaceAddress}/platform/express/status",
                        HttpMethod = HttpMethod.GET.ToString(),
                        Parameters = new List<CommomParamRequest>()
                        {
                            new CommomParamRequest()
                            {
                                Param="storeSapCode",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=true
                            },
                            new CommomParamRequest()
                            {
                                Param="trackingNos",
                                Value=string.Empty,
                                Placeholder=string.Empty,
                                Required=true
                            }
                        },
                        PostDatas=new List<object>()
                    }
                };
            }
            return _result;
        }

        /// <summary>
        /// API请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OMSApiResultResponse GetTwApiResult(OMSApiToolRequest request)
        {
            try
            {
                OMSApiResultResponse _result = new OMSApiResultResponse();

                if (request.apiPath < 1 || request.apiPath > 7)
                {
                    throw new Exception("Please select a API Path!");
                }

                if (request.httpMethod == HttpMethod.POST.ToString())
                {
                    if (string.IsNullOrEmpty(request.postData))
                    {
                        throw new Exception("Please input a data!");
                    }
                }

                //密钥
                var _secret = this.GetAccessToken(request.ApiID);

                //参数
                var _subParams = JsonSerializer.Deserialize<List<CommomParamRequest>>(request.paramArray);
                //GET
                if (request.httpMethod.ToUpper().Equals(HttpMethod.GET.ToString()))
                {
                    IDictionary<string, string> _params = new Dictionary<string, string>();
                    //默认参数
                    _params.Add("userid", request.UserID);
                    _params.Add("version", request.Version);
                    _params.Add("format", request.Format);
                    _params.Add("method", request.Method);
                    _params.Add("timestamp", TimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString());
                    //传递参数
                    foreach (var item in _subParams)
                    {
                        _params.Add(item.Param, item.Value.ToString());
                    }
                    //签名
                    _params.Add("sign", CreateSign(_params, _secret, request.Method));
                    //发起请求
                    var _res = this.DoGet(request.requestURL, _params);
                    _result.RequestInfo = $"{request.httpMethod.ToUpper()} {_res.RequestUrl}";
                    _result.ResponseInfo = VariableHelper.ConvertJsonString(_res.ResponseInfo);
                }
                //POST
                else if (request.httpMethod.ToUpper().Equals(HttpMethod.POST.ToString()))
                {
                    IDictionary<string, string> _params = new Dictionary<string, string>();
                    //默认参数
                    _params.Add("userid", request.UserID);
                    _params.Add("version", request.Version);
                    _params.Add("format", request.Format);
                    _params.Add("method", request.Method);
                    _params.Add("timestamp", TimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString());
                    //签名
                    _params.Add("sign", CreateSign(_params, _secret, request.Method));
                    //发起请求
                    var _res = this.DoPost(request.requestURL, _params, request.postData);
                    _result.RequestInfo = $"{request.httpMethod.ToUpper()} {_res.RequestUrl}\r\n{VariableHelper.ConvertJsonString(_res.RequestData)}";
                    _result.ResponseInfo = VariableHelper.ConvertJsonString(_res.ResponseInfo);
                }

                //返回信息
                _result.Result = true;
                _result.Message = string.Empty;
                return _result;
            }
            catch (Exception ex)
            {
                //返回信息
                return new OMSApiResultResponse()
                {
                    Result = false,
                    Message = ex.Message,
                    RequestInfo = "",
                    ResponseInfo = ""
                };
            }
        }
        #endregion

        #region 函数
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="apiID"></param>
        /// <returns></returns>
        private string GetAccessToken(int apiID)
        {
            return _appDB.ApplicationApiDetail.Where(p => p.ID == apiID).SingleOrDefault().AccessToken;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        private OMSApiRequestInfo DoGet(string url, IDictionary<string, string> requestParams)
        {
            try
            {
                OMSApiRequestInfo _result = new OMSApiRequestInfo();
                string _params = string.Empty;
                foreach (var _item in requestParams)
                {
                    _params += $"&{_item.Key}={_item.Value}";
                }
                _params = _params.Substring(1);
                _result.RequestUrl = $"{url}?{_params}";
                HttpWebRequest req = null;
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(TrustAllValidationCallback);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(_result.RequestUrl));
                }
                else
                {
                    req = (HttpWebRequest)WebRequest.Create(_result.RequestUrl);
                }
                req.Method = WebRequestMethods.Http.Get;
                //req.KeepAlive = true;
                req.Timeout = 0xea60;
                req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                //发送请求
                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        _result.ResponseInfo = sr.ReadToEnd();
                    }
                }
                return _result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestParams"></param>
        /// <param name="postData"></param>
        private OMSApiRequestInfo DoPost(string url, IDictionary<string, string> requestParams, string postData)
        {
            try
            {
                OMSApiRequestInfo _result = new OMSApiRequestInfo();
                string _params = string.Empty;
                foreach (var _item in requestParams)
                {
                    _params += $"&{_item.Key}={_item.Value}";
                }
                _params = _params.Substring(1);
                _result.RequestUrl = $"{url}?{_params}";
                //获取信息
                HttpWebRequest req = null;
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(TrustAllValidationCallback);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(_result.RequestUrl));
                }
                else
                {
                    req = (HttpWebRequest)WebRequest.Create(_result.RequestUrl);
                }

                req.ReadWriteTimeout = 5 * 1000;
                req.Method = WebRequestMethods.Http.Post;
                req.ContentType = "application/json";
                _result.RequestData = postData;
                //发送请求
                using (var sw = new StreamWriter(req.GetRequestStream()))
                {
                    sw.Write(_result.RequestData);
                }
                using (var response = req.GetResponse())
                {
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        _result.ResponseInfo = sr.ReadToEnd();
                    }
                }
                return _result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool TrustAllValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; // ignore ssl certificate check
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="secret"></param>
        /// <param name="encryptMethod"></param>
        /// <returns></returns>
        public static string CreateSign(IDictionary<string, string> parameters, string secret, string encryptMethod)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            string _query = string.Empty;
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                //过滤null和空的字符
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    _query += $"{key}{value}";
                }
            }

            // 第三步：使用HMAC加密
            byte[] bytes;
            if (OMSApiToolInitializeResponse.SIGN_METHOD_SHA256.Equals(encryptMethod))
            {
                HMACSHA256 _provider = new HMACSHA256();
                _provider.Key = Encoding.UTF8.GetBytes(secret);
                bytes = _provider.ComputeHash(Encoding.UTF8.GetBytes(_query));
            }
            else
            {
                HMACMD5 hmac = new HMACMD5(Encoding.UTF8.GetBytes(secret));
                bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(_query));
            }

            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }
    }
    #endregion
}
