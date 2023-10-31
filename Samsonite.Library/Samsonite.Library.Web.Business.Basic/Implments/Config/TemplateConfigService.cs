using Microsoft.EntityFrameworkCore;
using Samsonite.Library.Data.Entity.Models;
using Samsonite.Library.Utility;
using Samsonite.Library.Web.Business.Basic.Models;
using Samsonite.Library.Web.Core;
using Samsonite.Library.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Samsonite.Library.Web.Business.Basic
{
    public class TemplateConfigService : ITemplateConfigService
    {
        private IBaseService _baseService;
        private appEntities _appDB;
        public TemplateConfigService(IBaseService baseService, appEntities appEntities)
        {
            _baseService = baseService;
            _appDB = appEntities;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryResponse<SysTemplate> GetQuery(TemplateConfigSearchRequest request)
        {
            QueryResponse<SysTemplate> _result = new QueryResponse<SysTemplate>();
            var _list = _appDB.SysTemplate.AsQueryable();

            //搜索条件
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                _list = _list.Where(p => p.TemplateName.Contains(request.Keyword) || p.TemplateIndentify.Contains(request.Keyword));
            }

            if (request.Type > 0)
            {
                _list = _list.Where(p => p.TemplateType == request.Type);
            }

            //返回数据
            request.Page = VariableHelper.SaferequestPage(request.Page);
            _result.TotalRecord = _list.Count();
            _result.Items = _list.AsNoTracking().OrderBy(p => p.ID).Skip((request.Page - 1) * request.Rows).Take(request.Rows).ToList();
            return _result;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PostResponse Add(TemplateConfigAddRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    throw new Exception("模板名称不能为空");
                }

                if (string.IsNullOrEmpty(request.Indentify))
                {

                    throw new Exception("模板标识不能为空");
                }
                else
                {
                    SysTemplate objSysSysTemplate = _appDB.SysTemplate.Where(p => p.TemplateIndentify == request.Indentify).SingleOrDefault();
                    if (objSysSysTemplate != null)
                    {
                        throw new Exception("模板标识已经存在，请勿重复");
                    }
                }

                if (request.Type == 0)
                {
                    throw new Exception("请选择模板类型");
                }

                if (string.IsNullOrEmpty(request.Content))
                {
                    throw new Exception("模板内容不能为空");
                }

                SysTemplate objData = new SysTemplate()
                {
                    TemplateIndentify = request.Indentify,
                    TemplateName = request.Name,
                    TemplateType = request.Type,
                    TemplateTitle = request.Title,
                    TemplateContent = request.Content,
                    TemplateSender = request.Sender,
                    Remark = request.Remark,
                    CreateTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                _appDB.SysTemplate.Add(objData);
                _appDB.SaveChanges();
                //添加日志
                _baseService.InsertLog<SysTemplate>(objData, objData.ID.ToString());
                //返回信息
                return new PostResponse()
                {
                    Result = true,
                    Message = "数据保存成功"
                };
            }
            catch (Exception ex)
            {
                //返回信息
                return new PostResponse()
                {
                    Result = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PostResponse Edit(TemplateConfigEditRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                {
                    throw new Exception("模板名称不能为空");
                }

                if (string.IsNullOrEmpty(request.Indentify))
                {

                    throw new Exception("模板标识不能为空");
                }
                else
                {
                    SysTemplate objSysSysTemplate = _appDB.SysTemplate.Where(p => p.TemplateIndentify == request.Indentify && p.ID != request.ID).SingleOrDefault();
                    if (objSysSysTemplate != null)
                    {
                        throw new Exception("模板标识已经存在，请勿重复");
                    }
                }

                if (request.Type == 0)
                {
                    throw new Exception("请选择模板类型");
                }

                if (string.IsNullOrEmpty(request.Content))
                {
                    throw new Exception("模板内容不能为空");
                }

                SysTemplate objData = _appDB.SysTemplate.Where(p => p.ID == request.ID).SingleOrDefault();
                if (objData != null)
                {
                    objData.TemplateIndentify = request.Indentify;
                    objData.TemplateName = request.Name;
                    objData.TemplateType = request.Type;
                    objData.TemplateTitle = request.Title;
                    objData.TemplateContent = request.Content;
                    objData.TemplateSender = request.Sender;
                    objData.Remark = request.Remark;
                    objData.EditTime = DateTime.Now;
                    _appDB.SaveChanges();
                    //添加日志
                    _baseService.UpdateLog<SysTemplate>(objData, objData.ID.ToString());
                    //返回信息
                    return new PostResponse()
                    {
                        Result = true,
                        Message = "数据保存成功"
                    };
                }
                else
                {
                    throw new Exception("数据读取失败");
                }
            }
            catch (Exception ex)
            {
                //返回信息
                return new PostResponse()
                {
                    Result = false,
                    Message = ex.Message
                };
            }
        }

        /// <summary>
        /// 根据模板标识获取模板
        /// </summary>
        /// <param name="indentify"></param>
        /// <returns></returns>
        public SysTemplate GetTemplate(string indentify)
        {
            return _appDB.SysTemplate.Where(p => p.TemplateIndentify == indentify).SingleOrDefault();
        }

        /// <summary>
        /// 模板类型列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> TemplateTypeObject()
        {
            Dictionary<int, string> _result = new Dictionary<int, string>();
            _result.Add((int)TemplateType.Email, "邮件");
            _result.Add((int)TemplateType.SMS, "短信");
            return _result;
        }
    }
}
